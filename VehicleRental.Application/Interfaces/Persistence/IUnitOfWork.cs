using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRental.Application.Interfaces.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IMotorcycleRepository Motorcycles { get; }
        IRentalRepository Rentals { get; }
        IDriverRepository Drivers { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
