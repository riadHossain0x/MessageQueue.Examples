using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace RabbitMQConsumer.MassTransit.Controllers;

[ApiController]
[Produces("application/json")]
public class BookingController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IBus _bus;

    public BookingController(IPublishEndpoint publishEndpoint, IBus bus)
    {
        _publishEndpoint = publishEndpoint;
        _bus = bus;
    }

    //[HttpPost]
    //[Route("No")]
    //public async Task<IActionResult> Create([FromBody] Order order)
    //{
    //    await _publishEndpoint.Publish<Order>(order);

    //    //var endpoint = await _bus.GetSendEndpoint(new Uri("queue:order-submitted"));
    //    //await endpoint.Send(order);
    //    return Ok(order);
    //}
}
