using Microsoft.AspNetCore.Mvc;
using VehicleRental.Application.DTOs.Rental;
using VehicleRental.Application.DTOs.Rentals;
using VehicleRental.Application.Interfaces;
using VehicleRental.Application.Interfaces.Persistence;
using VehicleRental.Domain.Entities;
using VehicleRental.Domain.Enums;

namespace VehicleRental.Api.Controllers
{
    [ApiController]
    [Route("locacao")]
    public class RentalsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRentalCostCalculator _costCalculator;

        public RentalsController(IUnitOfWork unitOfWork, IRentalCostCalculator costCalculator)
        {
            _unitOfWork = unitOfWork;
            _costCalculator = costCalculator;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromBody] CreateRentalRequest request)
        {
            decimal dailyRate;
            var driver = await _unitOfWork.Drivers.GetByIdAsync(request.DriverId);
            var motorcycle = await _unitOfWork.Motorcycles.GetByIdAsync(request.MotorcycleId);

            if (driver == null)
                return NotFound(new { mensagem = "Entregador não encontrado." });

            if (driver.CnhType == CnhType.B)
                return BadRequest(new { mensagem = "Entregador não possui habilitação tipo A ou AB." });

            if (await _unitOfWork.Rentals.DriverHasActiveRentalAsync(driver.Identifier))
                return BadRequest(new { mensagem = "Entregador já possui uma locação ativa." });

            if (motorcycle == null)
                return NotFound(new { mensagem = "Moto não encontrada." });

            if (await _unitOfWork.Rentals.HasRentalsByMotorcycleIdAsync(motorcycle.Identifier))
                return BadRequest(new { mensagem = "Moto já está alugada." });
            
            try
            {
                dailyRate = _costCalculator.GetPlanRate(request.PlanDays);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { mensagem = "O plano escolhido não é válido" });
            }
            
            if (request.StartDate.Date != DateTime.Now.AddDays(1).Date)
                return BadRequest(new { mensagem = "A data de início da locação deve ser o primeiro dia após a data atual." });

            var expectedEndDate = request.StartDate.AddDays(request.PlanDays - 1).Date;
            if (request.ExpectedEndDate.Date != expectedEndDate || request.EndDate.Date != expectedEndDate)
                return BadRequest(new { mensagem = $"Datas de término/previsão inválidas. Para o plano de {request.PlanDays} dias, deve ser {expectedEndDate:yyyy-MM-dd}." });


            var rental = new Rental(
                driver.Identifier,
                motorcycle.Identifier,
                request.PlanDays,
                request.StartDate,
                request.EndDate,
                request.ExpectedEndDate
            );

            await _unitOfWork.Rentals.AddAsync(rental);
            await _unitOfWork.SaveChangesAsync();

            var response = new RentalResponse(rental, dailyRate);

            return CreatedAtAction(nameof(GetById), new { id = rental.Identifier }, response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(RentalResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var rental = await _unitOfWork.Rentals.GetByIdAsync(id);
            if (rental == null)
            {
                return NotFound(new { mensagem = "Locação não encontrada" });
            }

            var dailyRate = _costCalculator.GetPlanRate(rental.PlanDays);
            var response = new RentalResponse(rental, dailyRate);

            return Ok(response);
        }

        [HttpPut("{id}/devolucao")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Return(string id, [FromBody] ReturnRentalRequest request)
        {
            var rental = await _unitOfWork.Rentals.GetByIdAsync(id);
            if (rental == null)
            {
                return BadRequest(new { mensagem = "Locação não encontrada" });
            }

            if (rental.ReturnDate != null)
            {
                return BadRequest(new { mensagem = "Locação já foi devolvida." });
            }

            // Calcula o custo total
            var totalCost = _costCalculator.CalculateCost(rental, request.ReturnDate);

            // Atualiza a entidade
            rental.CompleteRental(request.ReturnDate, totalCost);
            await _unitOfWork.SaveChangesAsync();

            // Retorna o custo total e a mensagem
            return Ok(new
            {
                mensagem = "Data de devolução informada com sucesso"
            });
        }

    }
}
