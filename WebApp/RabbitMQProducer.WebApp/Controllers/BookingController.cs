using Microsoft.AspNetCore.Mvc;
using RabbitMQProducer.WebApp.Models;
using RabbitMQProducer.WebApp.Services;
using System.Text.Json;

namespace RabbitMQProducer.WebApp.Controllers;

[ApiController]
[Produces("application/json")]
public class BookingController : ControllerBase
{
    private readonly ILogger<BookingController> _logger;
    private readonly IPublisher publisher;

    public BookingController(ILogger<BookingController> logger, IPublisher publisher)
    {
        _logger = logger;
        this.publisher = publisher;
    }

    [HttpPost]
    [Route("Create")]
    public IActionResult Create([FromBody] BookingModel model)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var jsonString = JsonSerializer.Serialize(model);
        publisher.Publish(jsonString, "report.order", null, null);

        return Ok();
    }
}
