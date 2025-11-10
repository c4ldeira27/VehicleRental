using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRental.Domain.Entities
{
    public class Notification
    {
        public Guid Id { get; private set; }
        public string MotorcycleId { get; private set; } // Referencia o Identifier da moto
        public int MotorcycleYear { get; private set; }
        public string MotorcyclePlate { get; private set; }
        public DateTime NotifiedAt { get; private set; }

        public Notification() { }

        public Notification(string motorcycleId, int motorcycleYear, string motorcyclePlate)
        {
            Id = Guid.NewGuid();
            MotorcycleId = motorcycleId;
            MotorcycleYear = motorcycleYear;
            MotorcyclePlate = motorcyclePlate;
            NotifiedAt = DateTime.UtcNow;
        }
    }
}
