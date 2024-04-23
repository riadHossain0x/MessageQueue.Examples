using MassTransit.Models;
using MassTransit;

namespace RabbitMQConsumer.MassTransit.Consumers;

public class OrderConsumer : IConsumer<Order>
{
    private readonly IServiceProvider serviceProvider;

    public OrderConsumer(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task Consume(ConsumeContext<Order> context)
    {
        await Console.Out.WriteLineAsync(context.Message.Name);
    }
}

public class OrderConsumer2 : IConsumer<Order>
{
    public async Task Consume(ConsumeContext<Order> context)
    {
        await Console.Out.WriteLineAsync(context.Message.Name);
    }
}
