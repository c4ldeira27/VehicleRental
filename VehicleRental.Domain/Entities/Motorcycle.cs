using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRental.Domain.Entities
{
    public class Motorcycle
    {
        [Key]
        public string Identifier { get; private set; }
        public int Year { get; private set; }
        public string Model { get; private set; }
        public string Plate { get; private set; }

        public ICollection<Rental> Rentals { get; private set; } = new List<Rental>();

        public Motorcycle() { }

        public Motorcycle(string identifier, int year, string model, string plate)
        {
            Identifier = identifier;
            Year = year;
            Model = model;
            Plate = plate;
        }

        public void UpdatePlate(string newPlate)
        {
            Plate = newPlate;
        }

    }
}
