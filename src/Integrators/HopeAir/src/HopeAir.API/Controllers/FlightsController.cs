using HopeAir.Application.Features.Flight.Queries.SearchFlights;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace HopeAir.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FlightsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("search")]
    public async Task<IActionResult> SearchFlights([FromQuery] SearchFlightsQuery request)
    {
        var result = await _mediator.Send(request);
        return Ok(result);
    }
}