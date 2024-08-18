using System.Transactions;
using AccountMicroservice.Application.Mappers;
using AccountMicroservice.Application.Services.gRPC;
using AccountMicroservice.Application.Services.RecurrentTransactionService;
using AccountMicroservice.Domain.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(TransactionProfile));
builder.Services.AddAutoMapper(typeof(AccountProfile));
// Register your DbContext with the necessary database provider
builder.Services.AddDbContext<AccountDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Host=localhost:5433;Database=Account_db;Username=username;Password=mysecretpassword")));
//Register RabbitMqConsumers
builder.Services.AddHostedService<RabbitMqTransactionsConsumerService>();
builder.Services.AddHostedService<RabbitMqAccountConsumerService>();
builder.Services.AddControllers();

//Register gRPC server
builder.Services.AddGrpc();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapControllers();
app.UseRouting();
app.UseAuthorization();
app.UseGrpcWeb();
app.MapGrpcService<AccountServiceImpl>().EnableGrpcWeb();
app.MapGrpcService<gRPCTransactionImpl>().EnableGrpcWeb();
app.MapGrpcService<gRPCAccountImpl>().EnableGrpcWeb();
app.MapGrpcService<gRPCRollbackService>().EnableGrpcWeb();
app.UseCors("AllowAll");

app.Run();