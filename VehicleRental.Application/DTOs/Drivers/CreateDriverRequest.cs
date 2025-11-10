using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VehicleRental.Application.DTOs.Drivers
{
    public class CreateDriverRequest
    {
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; }

        [JsonPropertyName("nome")]
        public string Name { get; set; }

        [JsonPropertyName("cnpj")]
        public string Cnpj { get; set; }

        [JsonPropertyName("data_nascimento")]
        public DateTime DateOfBirth { get; set; }

        [JsonPropertyName("numero_cnh")]
        public string CnhNumber { get; set; }

        [JsonPropertyName("tipo_cnh")]
        public string CnhType { get; set; }

        [JsonPropertyName("imagem_cnh")]
        public string? CnhImageBase64 { get; set; }
    }
}
