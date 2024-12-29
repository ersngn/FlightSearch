using FlightSearch.Application.Interfaces;
using FlightSearch.Core.Response;
using MediatR;

namespace FlightSearch.Application.Feature.Flights.Queries.SearchFlights;

public class
    SearchFlightsQueryHandler : IRequestHandler<SearchFlightsQuery,
        ApiResponse<IEnumerable<SearchFlightsQueryResponse>>>
{
    private readonly IProviderService _providerService;

    public SearchFlightsQueryHandler(IProviderService providerService)
    {
        _providerService = providerService;
    }

    public async Task<ApiResponse<IEnumerable<SearchFlightsQueryResponse>>> Handle(SearchFlightsQuery request,
        CancellationToken cancellationToken)
    {
        var providersResult = await _providerService.SearchFlightsAsync(request);


        if (!providersResult.Any())
        {
            return ApiResponse<IEnumerable<SearchFlightsQueryResponse>>.SuccessApiResponse(null,
                "No flights found for the given criteria.");
        }

        var response = providersResult.Where(f => f.Departure == request.Origin && f.Arrival == request.Destination &&
                                                  f.DepartureTime >= request.DepartureDate &&
                                                  f.ArrivalTime <= request.ReturnDate).Select(e =>
            new SearchFlightsQueryResponse()
            {
                Arrival = e.Arrival,
                ArrivalTime = e.ArrivalTime,
                Currency = e.Currency,
                Departure = e.Departure,
                DepartureTime = e.DepartureTime,
                Duration = e.Duration,
                FlightNumber = e.FlightNumber,
                Price = e.Price,
                ProviderName = e.ProviderName
            });

        return !response.Any()
            ? ApiResponse<IEnumerable<SearchFlightsQueryResponse>>.SuccessApiResponse(
                new List<SearchFlightsQueryResponse>(), "No flights found.")
            : ApiResponse<IEnumerable<SearchFlightsQueryResponse>>.SuccessApiResponse(response);
    }
}