using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VehicleRental.Application.DTOs.Motorcycles
{
    public class UpdateMotorcyclePlateRequest
    {
        [JsonPropertyName("placa")]
        public string Plate { get; set; }
    }
}
