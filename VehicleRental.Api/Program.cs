using Microsoft.EntityFrameworkCore;
using VehicleRental.Api.Workers;
using VehicleRental.Application.Interfaces;
using VehicleRental.Application.Interfaces.Persistence;
using VehicleRental.Application.Services;
using VehicleRental.Infrastructure.Data;
using VehicleRental.Infrastructure.Messaging;
using VehicleRental.Infrastructure.Repositories;
using VehicleRental.Infrastructure.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Backend Challenge",
        Version = "v1",
        Description = "API para gerenciamento de aluguel de motos e entregadores."
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<IMessageBusService, RabbitMqService>();
builder.Services.AddScoped<IStorageService, LocalStorageService>();
builder.Services.AddScoped<IRentalCostCalculator, RentalCostCalculator>();

builder.Services.AddHostedService<MotorcycleNotificationConsumer>();

var app = builder.Build();

try
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }
}
catch (Exception ex)
{
    Console.WriteLine("Ocorreu um erro ao aplicar as migrações do banco de dados.");
    Console.WriteLine(ex.Message);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rental API V1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
