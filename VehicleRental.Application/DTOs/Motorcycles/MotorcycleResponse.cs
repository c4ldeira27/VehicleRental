using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VehicleRental.Application.DTOs.Motorcycles
{
    public class MotorcycleResponse
    {
        [JsonPropertyName("identificador")]
        public string Identifier { get; set; }

        [JsonPropertyName("ano")]
        public int Year { get; set; }

        [JsonPropertyName("modelo")]
        public string Model { get; set; }

        [JsonPropertyName("placa")]
        public string Plate { get; set; }

        public MotorcycleResponse(string identifier, int year, string model, string plate)
        {
            Identifier = identifier;
            Year = year;
            Model = model;
            Plate = plate;
        }
    }
}
