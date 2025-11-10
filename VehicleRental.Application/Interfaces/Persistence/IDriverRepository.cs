using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRental.Domain.Entities;

namespace VehicleRental.Application.Interfaces.Persistence
{
    public interface IDriverRepository
    {
        Task AddAsync(Driver driver);
        Task<Driver?> GetByIdAsync(string id);
        Task<bool> CnpjExistsAsync(string cnpj);
        Task<bool> CnhNumberExistsAsync(string cnhNumber);
        void Update(Driver driver);
    }
}
