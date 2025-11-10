using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using VehicleRental.Domain.Entities;
using VehicleRental.Infrastructure.Data;

namespace VehicleRental.Api.Workers
{
    public class MotorcycleNotificationConsumer : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private IConnection _connection;
        private IModel _channel;
        private const string QueueName = "motorcycle-created";
        public MotorcycleNotificationConsumer(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;

            InitializeRabbitMq();
        }


        private void InitializeRabbitMq()
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMq:Host"],
                UserName = _configuration["RabbitMq:Username"],
                Password = _configuration["RabbitMq:Password"]
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: QueueName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                await ProcessMessageAsync(message);
            };

            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(string message)
        {
            try
            {
                // Deserializa o JSON que enviamos no Controller
                using var document = JsonDocument.Parse(message);
                var root = document.RootElement;

                var year = root.GetProperty("Year").GetInt32();

                // REGRA DE NEGÓCIO: Notificar se o ano for 2024
                if (year == 2024)
                {
                    var id = root.GetProperty("Id").GetString();
                    var plate = root.GetProperty("Plate").GetString();

                    // Cria um escopo novo para poder usar o UnitOfWork (que é Scoped)
                    // dentro de um Singleton (BackgroundService)
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                        // Como Notification é muito simples, podemos usar o DbContext direto
                        // ou adicionar um repositório para ela se preferir.
                        // Para simplificar, vamos adicionar direto aqui:
                        var notification = new Notification(id!, year, plate!);

                        dbContext.Notifications.Add(notification);
                        await dbContext.SaveChangesAsync();

                        Console.WriteLine($"[NOTIFICATION] Moto 2024 cadastrada! Placa: {plate}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");
            }
        }

        public override void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
            base.Dispose();
        }
    }
}
