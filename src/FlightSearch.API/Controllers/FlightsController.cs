using FlightSearch.Application.Feature.Flights.Queries.SearchFlights;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FlightSearch.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class FlightsController : ControllerBase
{
    private readonly IMediator _mediator;

    public FlightsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchFlights([FromQuery] SearchFlightsQuery query)
    {

        var result = await _mediator.Send(query);
        
        return Ok(result);
    }
}