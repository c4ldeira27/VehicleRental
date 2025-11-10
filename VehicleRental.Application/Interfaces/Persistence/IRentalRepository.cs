using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRental.Domain.Entities;

namespace VehicleRental.Application.Interfaces.Persistence
{
    public interface IRentalRepository
    {
        Task<bool> HasRentalsByMotorcycleIdAsync(string motorcycleId);
        Task<Rental?> GetByIdAsync(string id);
        Task<bool> DriverHasActiveRentalAsync(string driverId);
        Task AddAsync(Rental rental);
    }
}
