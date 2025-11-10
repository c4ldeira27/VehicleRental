using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRental.Application.Interfaces
{
    public interface IStorageService
    {
        Task<string> SaveFileAsync(string base64Image, string fileName);
    }
}
