using Microsoft.AspNetCore.Mvc;
using VehicleRental.Application.DTOs.Motorcycles;
using VehicleRental.Application.Interfaces;
using VehicleRental.Application.Interfaces.Persistence;
using VehicleRental.Domain.Entities;

namespace VehicleRental.Api.Controllers
{
    [ApiController]
    [Route("motos")]
    public class MotorcyclesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageBusService _messageBus;
        public MotorcyclesController(IUnitOfWork unitOfWork, IMessageBusService messageBus)
        {
            _unitOfWork = unitOfWork;
            _messageBus = messageBus;
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateMotorcycleRequest request)
        {
            if(await _unitOfWork.Motorcycles.PlateExistsAsync(request.Plate))
            {
                return BadRequest(new { mensagem = "Placa já cadastrada" });
            }

            var motorcycle = new Motorcycle(request.Identifier, request.Year, request.Model, request.Plate);

            await _unitOfWork.Motorcycles.AddAsync(motorcycle);
            await _unitOfWork.SaveChangesAsync();

            var eventMessage = new
            {
                Id = motorcycle.Identifier,
                Year = motorcycle.Year,
                Plate = motorcycle.Plate
            };

            _messageBus.Publish("motorcycle-created", eventMessage);

            return CreatedAtAction(nameof(GetById), new { id = motorcycle.Identifier }, null);
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MotorcycleResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] string? placa)
        {
            var motorcycles = await _unitOfWork.Motorcycles.FindByPlateAsync(placa);
            var response = motorcycles.Select(m => new MotorcycleResponse(
                m.Identifier,
                m.Year,
                m.Model,
                m.Plate
            ));

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MotorcycleResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            var motorcycle = await _unitOfWork.Motorcycles.GetByIdAsync(id);

            if (motorcycle == null) { 
                return NotFound(new {mensagem = "Moto não encontrada"});
            }

            return Ok(new MotorcycleResponse(
                motorcycle.Identifier,
                motorcycle.Year,
                motorcycle.Model,
                motorcycle.Plate
                ));
        }

        [HttpPut("{id}/placa")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePlate(string id, [FromBody] UpdateMotorcyclePlateRequest request)
        {
            var motorcycle = await _unitOfWork.Motorcycles.GetByIdAsync(id);
            if (motorcycle == null)
            {
                return NotFound(new { mensagem = "Moto não encontrada" });
            }

            var existingMotoWithPlate = await _unitOfWork.Motorcycles.GetByPlateAsync(request.Plate);
            if(existingMotoWithPlate != null && existingMotoWithPlate.Identifier != id)
            {
                return BadRequest(new { mensagem = "Placa já pertence a outra moto" });
            }

            motorcycle.UpdatePlate(request.Plate);
            _unitOfWork.Motorcycles.Update(motorcycle);
            await _unitOfWork.SaveChangesAsync();

            return Ok(new {mensagem = "Placa modificada com sucesso" });
        }


        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete(string id)
        {
            var motorcycle = await _unitOfWork.Motorcycles.GetByIdAsync(id);

            if (motorcycle == null)
                return NotFound(new { mensagem = "Moto não encontrada" });

            if (await _unitOfWork.Rentals.HasRentalsByMotorcycleIdAsync(id))
            {
                return BadRequest(new { mensagem = "Não é possível remover moto com locações associadas" });
            }

            _unitOfWork.Motorcycles.Delete(motorcycle);
            await _unitOfWork.SaveChangesAsync();

            return Ok();
        }
    }
}
