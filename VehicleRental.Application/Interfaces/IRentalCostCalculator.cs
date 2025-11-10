using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRental.Domain.Entities;

namespace VehicleRental.Application.Interfaces
{
    public interface IRentalCostCalculator
    {
        decimal GetPlanRate(int planDays);
        decimal CalculateCost(Rental rental, DateTime returnDate);
    }
}
