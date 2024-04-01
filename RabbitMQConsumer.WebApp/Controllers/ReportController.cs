using Microsoft.AspNetCore.Mvc;
using RabbitMQConsumer.WebApp.Models;

namespace RabbitMQConsumer.WebApp.Controllers;

[ApiController]
[Produces("application/json")]
public class ReportController : ControllerBase
{
    private readonly ILogger<ReportController> _logger;

    public ReportController(ILogger<ReportController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("Create")]
    public IActionResult Create([FromBody] OrderModel model)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);



        return Ok();
    }
}
