using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRental.Application.Interfaces.Persistence;
using VehicleRental.Domain.Entities;
using VehicleRental.Infrastructure.Data;

namespace VehicleRental.Infrastructure.Repositories
{
    public class RentalRepository : IRentalRepository
    {
        private readonly AppDbContext _context;

        public RentalRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> HasRentalsByMotorcycleIdAsync(string motorcycleId)
        {
            return await _context.Rentals.AnyAsync(r => r.MotorcycleId == motorcycleId);
        }

        public async Task<Rental?> GetByIdAsync(string id)
        {
            return await _context.Rentals.
                Include(r => r.Driver).
                Include(r => r.Motorcycle).
                FirstOrDefaultAsync(r => r.Identifier == id);
        }

        public async Task<bool> DriverHasActiveRentalAsync(string driverId)
        {
            return await _context.Rentals.AnyAsync(r => r.DriverId == driverId && r.EndDate == null);
        }

        public async Task AddAsync(Rental rental)
        {
            await _context.Rentals.AddAsync(rental);
        }
    }
}
