using MassTransit;
using RequestResponse.Models;

namespace Response.MassTransit.Consumers;

public class RequestConsumer : IConsumer<OrderRequestModel>
{
    public async Task Consume(ConsumeContext<OrderRequestModel> context)
    {
        await Console.Out.WriteLineAsync(context.Message.Id.ToString());

        await context.RespondAsync<OrderResponseModel>(new
        {
            Id = context.Message.Id,
            Count = 10
        });
    }
}
