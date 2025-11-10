using Microsoft.AspNetCore.Mvc;
using VehicleRental.Application.DTOs.Drivers;
using VehicleRental.Application.Interfaces;
using VehicleRental.Application.Interfaces.Persistence;
using VehicleRental.Domain.Entities;
using VehicleRental.Domain.Enums;

namespace VehicleRental.Api.Controllers
{
    [ApiController]
    [Route("entregadores")]
    public class DriversController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStorageService _storageService;

        public DriversController(IUnitOfWork unitOfWork, IStorageService storageService)
        {
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateDriverRequest request)
        {
            if (await _unitOfWork.Drivers.CnpjExistsAsync(request.Cnpj))
            {
                return BadRequest(new { mensagem = "CNPJ já cadastrado" });
            }

            if (await _unitOfWork.Drivers.CnhNumberExistsAsync(request.CnhNumber))
            {
                return BadRequest(new { mensagem = "CNH já cadastrada" });
            }

            if (!Enum.TryParse<CnhType>(request.CnhType, out var cnhType))
            {
                return BadRequest(new { mensagem = "Tipo de CNH inválido. Use A, B ou AB" });
            }

            var driver = new Driver(
                request.Identifier,
                request.Name,
                request.Cnpj,
                request.DateOfBirth,
                request.CnhNumber,
                cnhType
            );

            if (!string.IsNullOrEmpty(request.CnhImageBase64))
            {
                var fileName = $"{driver.Identifier}_cnh.png";
                var path = await _storageService.SaveFileAsync(request.CnhImageBase64, fileName);
                driver.UpdateCnhImage(path);
            }

            await _unitOfWork.Drivers.AddAsync(driver);
            await _unitOfWork.SaveChangesAsync();

            return Created("", new { mensagem = "Entregador cadastrado com sucesso" });
        }

        [HttpPost("{id}/cnh")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadCnh(string id, [FromBody] UpdateCnhRequest request)
        {
            var driver = await _unitOfWork.Drivers.GetByIdAsync(id);

            if (driver == null)
            {
                return NotFound(new { mensagem = "Entregador não encontrado" });
            }

            if (string.IsNullOrEmpty(request.CnhImageBase64))
            {
                return BadRequest(new { mensagem = "Imagem inválida" });
            }

            try
            {
                var fileName = $"{driver.Identifier}_cnh.png";
                var fullPath = await _storageService.SaveFileAsync(request.CnhImageBase64, fileName);

                driver.UpdateCnhImage(fullPath);
                _unitOfWork.Drivers.Update(driver);
                await _unitOfWork.SaveChangesAsync();

                return Created("", new { mensagem = "CNH enviada com sucesso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { mensagem = "Erro ao salvar imagem. Verifique se é um Base64 válido." });
            }
        }


    }
}
