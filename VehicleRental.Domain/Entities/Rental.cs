using System.ComponentModel.DataAnnotations;

namespace VehicleRental.Domain.Entities
{
    public class Rental
    {
        [Key]
        public string Identifier { get; private set; }
        public string DriverId { get; private set; }
        public string MotorcycleId { get; private set; }
        public int PlanDays { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime ExpectedEndDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public DateTime? ReturnDate { get; private set; }
        public decimal? TotalCost { get; private set; }

        public Driver Driver { get; private set; }
        public Motorcycle Motorcycle { get; private set; }

        public Rental() { }

        public Rental(string driverId, string motorcycleId, int planDays, DateTime startDate, DateTime endDate,DateTime expectedEndDate)
        {
            Identifier = $"locacao_{Guid.NewGuid().ToString().Substring(0, 8)}";
            DriverId = driverId;
            MotorcycleId = motorcycleId;
            PlanDays = planDays;
            StartDate = startDate;
            EndDate = endDate;
            ExpectedEndDate = expectedEndDate;
            ReturnDate = null;
            TotalCost = null;
        }

        public void CompleteRental(DateTime returnDate, decimal totalCost)
        {
            ReturnDate = returnDate;
            TotalCost = totalCost;
        }
    }
}