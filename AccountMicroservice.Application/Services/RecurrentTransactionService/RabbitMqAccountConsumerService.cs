using System.Text;
using AccountMicroservice.Application.Services.RecurrentTransactionService.ViewModel;
using AccountMicroservice.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AccountMicroservice.Application.Services.RecurrentTransactionService;

public class RabbitMqAccountConsumerService :BackgroundService
{
    private readonly ConnectionFactory _factory;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public RabbitMqAccountConsumerService(IServiceScopeFactory scopeFactory, IMapper mapper)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
        _factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var connection = _factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "accountQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AccountDbContext>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var entity = JsonConvert.DeserializeObject<AccountViewModel>(message);

                Account acc = _mapper.Map<Account>(entity);
                acc.Id = 0;


                context.Accounts.Add(acc);
                await context.SaveChangesAsync();


                Console.WriteLine($" [x] Received entity with ID: {entity.Id}, AccountId: {entity.Name}");
            }
        };

        channel.BasicConsume(queue: "accountQueue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}