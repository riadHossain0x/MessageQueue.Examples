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
        try
        {
            throw new Exception("Error!");
            await Console.Out.WriteLineAsync(context.Message.Name);
        }
        catch (Exception ex)
        {
            await Console.Out.WriteLineAsync($"{ex.Message}, Retry count: {context.GetRetryAttempt()}");
            var retryCount = context.GetRetryAttempt();
            if (retryCount == 2)
            {
                Environment.Exit(retryCount);
            }
            throw;
        }
    }
}

public class OrderConsumer2 : IConsumer<Order>
{
    public async Task Consume(ConsumeContext<Order> context)
    {
        await Console.Out.WriteLineAsync(context.Message.Name);
    }
}
