using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRental.Domain.Entities;

namespace VehicleRental.Application.Interfaces.Persistence
{
    public interface IMotorcycleRepository
    {
        Task AddAsync(Motorcycle motorcycle);
        Task<Motorcycle?> GetByIdAsync(string id);
        Task<Motorcycle?> GetByPlateAsync(string plate);
        Task<bool> PlateExistsAsync(string plate);
        void Update(Motorcycle motorcycle);
        void Delete(Motorcycle motorcycle);
        Task<IEnumerable<Motorcycle>> FindByPlateAsync(string? plate);
    }
}
