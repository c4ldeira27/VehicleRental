using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using VehicleRental.Domain.Entities;


namespace VehicleRental.Application.DTOs.Rentals
{
    public class RentalResponse
    {
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; }

        [JsonPropertyName("valor_diaria")]
        public decimal DailyRate { get; set; }

        [JsonPropertyName("entregador_id")]
        public string DriverId { get; set; }

        [JsonPropertyName("moto_id")]
        public string MotorcycleId { get; set; }

        [JsonPropertyName("data_inicio")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("data_termino")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("data_previsao_termino")]
        public DateTime ExpectedEndDate { get; set; }

        [JsonPropertyName("data_devolucao")]
        public DateTime? ReturnDate { get; set; }

        public RentalResponse(VehicleRental.Domain.Entities.Rental rental, decimal dailyRate)
        {
            Identifier = rental.Identifier;
            DailyRate = dailyRate;
            DriverId = rental.DriverId;
            MotorcycleId = rental.MotorcycleId;
            StartDate = rental.StartDate;
            ExpectedEndDate = rental.ExpectedEndDate;
            EndDate = rental.EndDate;
            ReturnDate = rental.ReturnDate;
        }
    }
}
