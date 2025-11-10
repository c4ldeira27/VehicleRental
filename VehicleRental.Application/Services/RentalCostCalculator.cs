using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRental.Application.Interfaces;
using VehicleRental.Domain.Entities;

namespace VehicleRental.Application.Services
{
    public class RentalCostCalculator : IRentalCostCalculator
    {
        private readonly Dictionary<int, decimal> _planRates = new()
        {
            { 7, 30.00m },
            { 15, 28.00m },
            { 30, 22.00m },
            { 45, 20.00m },
            { 50, 18.00m }
        };

        public decimal GetPlanRate(int planDays)
        {
            if (_planRates.TryGetValue(planDays, out var rate))
            {
                return rate;
            }

            throw new ArgumentException("Plano de locação inválido.", nameof(planDays));
        }

        public decimal CalculateCost(Rental rental, DateTime returnDate)
        {
            var rate = GetPlanRate(rental.PlanDays);
            var expectedEndDate = rental.ExpectedEndDate;

            // Calcula os dias efetivamente utilizados (arredondando para cima)
            int daysUsed = (int)Math.Ceiling((returnDate.Date - rental.StartDate.Date).TotalDays);

            // 1. Devolução Atrasada
            if (returnDate.Date > expectedEndDate.Date)
            {
                int extraDays = (int)Math.Ceiling((returnDate.Date - expectedEndDate.Date).TotalDays);
                decimal planCost = rental.PlanDays * rate;
                decimal lateFee = extraDays * 50.00m;
                return planCost + lateFee;
            }

            // 2. Devolução Antecipada
            if (returnDate.Date < expectedEndDate.Date)
            {
                int daysUnused = (int)Math.Ceiling((expectedEndDate.Date - returnDate.Date).TotalDays);
                decimal usedDaysCost = daysUsed * rate;
                decimal penalty = 0;

                if (rental.PlanDays == 7)
                {
                    penalty = (daysUnused * rate) * 0.20m;
                }
                else if (rental.PlanDays == 15)
                {
                    penalty = (daysUnused * rate) * 0.40m;
                }
                else
                {
                    penalty = (daysUnused * rate) * 0.40m;
                }

                return usedDaysCost + penalty;
            }

            // 3. Devolução na Data Exata
            // Cobra apenas o valor total do plano
            return rental.PlanDays * rate;
        }
    }
}

