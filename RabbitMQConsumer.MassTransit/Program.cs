using MassTransit;
using MassTransit.Configuration;
using MassTransit.Models;
using RabbitMQ.Client;
using RabbitMQConsumer.MassTransit;
using RabbitMQConsumer.MassTransit.Consumers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");
        cfg.ConfigureEndpoints(ctx);

        cfg.Message<Order>(x => x.SetEntityName("order-exchange"));
        cfg.Publish<Order>(x => x.ExchangeType = ExchangeType.Fanout);

        cfg.ReceiveEndpoint($"order-queue-{Guid.NewGuid()}", x =>
        {
            x.ConfigureConsumeTopology = false;
            x.AutoDelete = true;
            x.M2Consumer<OrderConsumer>(() => { return new OrderConsumer(builder.Services.BuildServiceProvider()!); }, null);
            x.Bind("order-exchange", s =>
            {
                s.ExchangeType = ExchangeType.Fanout;
                //s.RoutingKey = "order-key";
            });
        });

        cfg.ReceiveEndpoint($"order-queue-{Guid.NewGuid()}", x =>
        {
            x.ConfigureConsumeTopology = false;
            x.AutoDelete = true;
            x.Consumer<OrderConsumer2>();
            x.Bind("order-exchange", s =>
            {
                s.ExchangeType = ExchangeType.Fanout;
                //s.RoutingKey = "order-key";
            });
        });
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
