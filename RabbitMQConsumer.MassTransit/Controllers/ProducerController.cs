using MassTransit;
using MassTransit.Models;
using Microsoft.AspNetCore.Mvc;

namespace RabbitMQConsumer.MassTransit.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProducerController : ControllerBase
{
    private readonly IBus _bus;
    private readonly IPublishEndpoint _publishEndpoint;

    public ProducerController(IBus bus, IPublishEndpoint publishEndpoint)
    {
        _bus = bus;
        _publishEndpoint = publishEndpoint;
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create([FromBody] Order order)
    {
        //await _publishEndpoint.Publish<Order>(order);
        var endpoint = await _bus.GetSendEndpoint(new Uri("exchange:order-exchange"));
        await endpoint.Send(order);
        return Ok(order);
    }
}
