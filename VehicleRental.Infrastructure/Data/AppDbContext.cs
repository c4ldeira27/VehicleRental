using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleRental.Domain.Entities;

namespace VehicleRental.Infrastructure.Data
{
    public  class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Domain.Entities.Driver> Drivers { get; set; }
        public DbSet<Domain.Entities.Motorcycle> Motorcycles { get; set; }
        public DbSet<Domain.Entities.Rental> Rentals { get; set; }
        public DbSet<Domain.Entities.Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Motorcycle>(entity =>
            {
                entity.HasKey(e => e.Identifier);
                entity.Property(e => e.Identifier).ValueGeneratedNever();
                entity.HasIndex(e => e.Plate).IsUnique();
                entity.Property(e => e.Model).IsRequired();
                entity.Property(e => e.Plate).IsRequired();
            });

            modelBuilder.Entity<Driver>(entity => {
                entity.HasKey(e => e.Identifier); 
                entity.Property(e => e.Identifier).ValueGeneratedNever();

                entity.HasIndex(e => e.Cnpj).IsUnique();
                entity.HasIndex(e => e.CnhNumber).IsUnique();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.CnhType)
                      .HasConversion<string>()
                      .HasMaxLength(3);
            });

            modelBuilder.Entity<Rental>(entity =>
            {
                entity.HasKey(e => e.Identifier);
                entity.Property(e => e.Identifier).ValueGeneratedNever();

                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
                entity.Property(e => e.ExpectedEndDate).IsRequired();
                entity.Property(e => e.ReturnDate).IsRequired(false);
                entity.Property(e => e.TotalCost).IsRequired(false);

                entity.HasOne(r => r.Driver)
                      .WithMany(d => d.Rentals)
                      .HasForeignKey(r => r.DriverId) 
                      .HasPrincipalKey(d => d.Identifier);

                entity.HasOne(r => r.Motorcycle)
                      .WithMany(m => m.Rentals)
                      .HasForeignKey(r => r.MotorcycleId)
                      .HasPrincipalKey(m => m.Identifier); 
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }
}
