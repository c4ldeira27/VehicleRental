using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRental.Application.Interfaces;

namespace VehicleRental.Infrastructure.Storage
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _storagePath;

        public LocalStorageService(IConfiguration configuration)
        {
            var folderName = configuration["Storage:LocalPath"] ?? "uploads";
            _storagePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }
        public async Task<string> SaveFileAsync(string base64Image, string fileName)
        {
            if(base64Image.Contains(","))
            {
                base64Image = base64Image.Split(',')[1];
            }

            byte[] imageBytes = Convert.FromBase64String(base64Image);
            string fullPath = Path.Combine(_storagePath, fileName);

            await File.WriteAllBytesAsync(fullPath, imageBytes);

            return fullPath;
        }
    }
}
