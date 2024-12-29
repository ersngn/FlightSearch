using FlightSearch.Core.Response;
using FlightSearch.Domain.Model;
using MediatR;

namespace FlightSearch.Application.Feature.Flights.Queries.SearchFlights;

public record SearchFlightsQuery(
        string Origin,
        string Destination,
        DateTime DepartureDate,
        DateTime ReturnDate,
        int PassengerCount) : IRequest<ApiResponse<IEnumerable<SearchFlightsQueryResponse>>>;