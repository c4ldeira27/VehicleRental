using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VehicleRental.Application.DTOs.Drivers
{
    public class UpdateCnhRequest
    {
        [JsonPropertyName("imagem_cnh")]
        public string CnhImageBase64 { get; set; }
    }
}
