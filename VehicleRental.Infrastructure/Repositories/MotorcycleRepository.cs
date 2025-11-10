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
    public class MotorcycleRepository : IMotorcycleRepository
    {
        private readonly AppDbContext _context;
        public MotorcycleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Motorcycle motorcycle)
        {
            await _context.Motorcycles.AddAsync(motorcycle);
        }

        public void Delete(Motorcycle motorcycle)
        {
            _context.Motorcycles.Remove(motorcycle);
        }

        public async Task<IEnumerable<Motorcycle>> FindByPlateAsync(string? plate)
        {
            var query = _context.Motorcycles.AsQueryable();

            if (!string.IsNullOrWhiteSpace(plate))
            {
                query = query.Where(m => m.Plate.Contains(plate));
            }

            return await query.ToListAsync();
        }

        public async Task<Motorcycle?> GetByIdAsync(string id)
        {
            return await _context.Motorcycles.FindAsync(id);
        }

        public async Task<Motorcycle?> GetByPlateAsync(string plate)
        {
            return await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.Plate == plate);
        }

        public async Task<bool> PlateExistsAsync(string plate)
        {
            return await _context.Motorcycles
                .AnyAsync(m => m.Plate == plate);
        }

        public void Update(Motorcycle motorcycle)
        {
            _context.Motorcycles.Update(motorcycle);
        }
    }
}
