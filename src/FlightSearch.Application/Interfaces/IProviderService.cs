using FlightSearch.Application.Feature.Flights.Queries.SearchFlights;
using FlightSearch.Core.Response;
using FlightSearch.Domain.Model;

namespace FlightSearch.Application.Interfaces;

public interface IProviderService
{
    Task<IEnumerable<Flight>> SearchFlightsAsync(SearchFlightsQuery query);
}