using System.Text;
using AccountMicroservice.Application.Services.RecurrentTransactionService.ViewModel;
using AccountMicroservice.Domain.Models;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqTransactionsConsumerService : BackgroundService
{
    private readonly ConnectionFactory _factory;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public RabbitMqTransactionsConsumerService(IServiceScopeFactory scopeFactory, IMapper mapper)
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

        channel.QueueDeclare(queue: "transactionQueue", durable: false, exclusive: false, autoDelete: false, arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AccountDbContext>();

                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var entity = JsonConvert.DeserializeObject<TransactionViewModel>(message);

                Transaction tran = _mapper.Map<Transaction>(entity);
                tran.Timestamp = DateTime.Now;

                var account = context.Accounts.FirstOrDefault(x => x.Id == tran.AccountId);
                if (account != null)
                {
                    account.Balance = tran.Deposit == true
                        ? account.Balance + tran.Amount
                        : account.Balance - tran.Amount;
                    tran.Id = 0;
                    context.Transactions.Add(tran);
                    await context.SaveChangesAsync();
                }

                Console.WriteLine($" [x] Received entity with ID: {entity.Id}, AccountId: {entity.AccountId}");
            }
        };

        channel.BasicConsume(queue: "transactionQueue", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }
}
