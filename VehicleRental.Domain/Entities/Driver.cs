using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRental.Domain.Entities
{
    public class Driver
    {
        [Key]
        public string Identifier { get; private set; }
        public string Name { get; private set; }
        public string Cnpj { get; private set; }
        public DateTime BirthDate { get; private set; }
        public string CnhNumber { get; private set; }
        public Enums.CnhType CnhType { get; private set; }
        public string? CnhImageUrl { get; private set; }
        public ICollection<Rental> Rentals { get; private set; } = new List<Rental>();

        public Driver() { }

        public Driver(string identifier, string name, string cnpj, DateTime birthDate, string cnhNumber, Enums.CnhType cnhType, string? cnhImageUrl = null)
        {
            Identifier = identifier;
            Name = name;
            Cnpj = cnpj;
            BirthDate = birthDate;
            CnhNumber = cnhNumber;
            CnhType = cnhType;
            CnhImageUrl = cnhImageUrl;
        }

        public void UpdateCnhImage(string imageUrl)
        {
            CnhImageUrl = imageUrl;
        }
    }
}
