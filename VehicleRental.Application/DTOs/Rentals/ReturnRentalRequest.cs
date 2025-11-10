using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VehicleRental.Application.DTOs.Rentals
{
    public class ReturnRentalRequest
    {
        [JsonPropertyName("data_devolucao")]
        public DateTime ReturnDate { get; set; }
    }
}
