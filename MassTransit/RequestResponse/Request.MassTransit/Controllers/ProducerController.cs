using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RequestResponse.Models;

namespace Request.MassTransit.Controllers;

[ApiController]
[Produces("application/json")]
public class ProducerController : ControllerBase
{
    private readonly IBus _bus;
    private readonly IRequestClient<OrderRequestModel> _requestClient;

    public ProducerController(IBus bus, IRequestClient<OrderRequestModel> requestClient)
    {
        _bus = bus;
        _requestClient = requestClient;
    }

    [HttpPost("Send")]
    public async Task<IActionResult> Send(OrderRequestModel order)
    {
        var response = await _requestClient.GetResponse<OrderResponseModel>(order);
        return Ok(response);
    }
}
