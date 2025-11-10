using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRental.Application.Interfaces.Persistence;
using VehicleRental.Infrastructure.Data;

namespace VehicleRental.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public IMotorcycleRepository Motorcycles { get; private set; }
        public IRentalRepository Rentals { get; private set; }
        public IDriverRepository Drivers { get; private set; }
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Motorcycles = new MotorcycleRepository(_context);
            Rentals = new RentalRepository(_context);
            Drivers = new DriverRepository(_context);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
