using FlightSearch.Core.Response;
using MediatR;

namespace HopeAir.Application.Features.Flight.Queries.SearchFlights;

public record SearchFlightsQuery(string Origin, string Destination, DateTime DepartureDate, DateTime ReturnDate,
    int PassengerCount) : IRequest<ApiResponse<IEnumerable<SearchFlightsQueryResponse>>>;