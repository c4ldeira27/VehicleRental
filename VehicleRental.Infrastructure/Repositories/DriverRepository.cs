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
    public class DriverRepository : IDriverRepository
    {
        private readonly AppDbContext _context;
        public DriverRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Driver driver)
        {
            await _context.Drivers.AddAsync(driver);
        }

        public async Task<bool> CnhNumberExistsAsync(string cnhNumber)
        {
            return await _context.Drivers.AnyAsync(c => c.CnhNumber == cnhNumber);
        }

        public async Task<bool> CnpjExistsAsync(string cnpj)
        {
            return await _context.Drivers.AnyAsync(d => d.Cnpj == cnpj);
        }

        public async Task<Driver?> GetByIdAsync(string id)
        {
            return await _context.Drivers.FirstOrDefaultAsync(d => d.Identifier == id);
        }

        public void Update(Driver driver)
        {
            _context.Drivers.Update(driver);
        }
    }
}
