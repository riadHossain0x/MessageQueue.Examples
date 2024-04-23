using MassTransit;
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
    var entryAssembly = Assembly.GetExecutingAssembly();
    config.AddConsumers(entryAssembly);
    //config.AddConsumer<OrderConsumer>();

    config.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("amqp://guest:guest@localhost:5672");
        cfg.ConfigureEndpoints(ctx);
        cfg.ReceiveEndpoint("order-queue", c =>
        {
            c.ConfigureConsumer<OrderConsumer>(ctx);
        });
    });
});

//builder.Services.AddMassTransit(config =>
//{
//    var entryAssembly = Assembly.GetExecutingAssembly();
//    config.AddConsumers(entryAssembly!);

//    config.UsingRabbitMq((ctx, cfg) =>
//    {
//        cfg.Host("amqp://guest:guest@localhost:5672");
//        cfg.ConfigureEndpoints(ctx);
//    });
//});

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
