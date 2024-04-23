using MassTransit;
using MassTransit.Models;
using Microsoft.AspNetCore.Mvc;

namespace RabbitMQProducer.MassTransit.Controllers;

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

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        //await _publishEndpoint.Publish<Order>(order);
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:Order"));
        await endpoint.Send(order);
        return Ok(order);
    }

    [HttpPost]
    [Route("Create2")]
    public async Task<IActionResult> Create2([FromBody] Order order)
    {
        //await _publishEndpoint.Publish<Order>(order);
        var endpoint = await _bus.GetSendEndpoint(new Uri("queue:Order2"));
        await endpoint.Send(order);
        return Ok(order);
    }
}
