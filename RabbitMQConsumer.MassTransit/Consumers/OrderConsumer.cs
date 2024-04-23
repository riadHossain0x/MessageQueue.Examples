using MassTransit.Models;
using MassTransit;

namespace RabbitMQConsumer.MassTransit.Consumers;

public class OrderConsumer : IConsumer<Order>
{
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

//public class OrderConsumerDefinition : ConsumerDefinition<OrderConsumer2>
//{
//    public OrderConsumerDefinition()
//    {
//        EndpointName = "Order2";
//    }
//}
