using AybJet.Application.Interfaces;
using FlightSearch.Core.Response;
using FluentValidation;
using MediatR;

namespace AybJet.Application.Feature.Flight.Queries.SearchFlights;

public class
    SearchFlightsQueryHandler : IRequestHandler<SearchFlightsQuery,
        ApiResponse<IEnumerable<SearchFlightsQueryResponse>>>
{
    private readonly IHttpClientService _httpClientService;
    private readonly IValidator<SearchFlightsQuery> _validator;

    public SearchFlightsQueryHandler(IHttpClientService httpClientService, IValidator<SearchFlightsQuery> validator)
    {
        _httpClientService = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
        _validator = validator ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<ApiResponse<IEnumerable<SearchFlightsQueryResponse>>> Handle(SearchFlightsQuery request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            return ApiResponse<IEnumerable<SearchFlightsQueryResponse>>.ValidationError("Validation failed.",
                validationResult);
        }

        try
        {
            var flights = await _httpClientService.PostAsync<Domain.Models.Flight>("mock-url", request);
            if (!flights.Any())
            {
                return ApiResponse<IEnumerable<SearchFlightsQueryResponse>>.SuccessApiResponse(
                    new List<SearchFlightsQueryResponse>(), "No flights found.");
            }

            var result = flights
                // .Where(f => f.Departure == request.Origin && f.Arrival == request.Destination &&
                //             f.DepartureTime >= request.DepartureDate && f.ArrivalTime <= request.ReturnDate)
                .Select(f => new SearchFlightsQueryResponse
                {
                    FlightNumber = f.FlightNumber,
                    Departure = f.Departure,
                    Arrival = f.Arrival,
                    Price = f.Price,
                    Currency = f.Currency,
                    DepartureTime = f.DepartureTime,
                    ArrivalTime = f.ArrivalTime,
                    Duration = f.Duration,
                    ProviderName = "AybJet"
                })
                .OrderBy(f => f.Price)
                .ToList();

            return ApiResponse<IEnumerable<SearchFlightsQueryResponse>>.SuccessApiResponse(result,
                "Flights retrieved successfully.", 200);
        }
        catch (Exception ex)
        {
            //TODO: Log mekanizması eklenmelidir.
            return ApiResponse<IEnumerable<SearchFlightsQueryResponse>>.Error(
                "An error occurred while fetching flights.", 500, new List<string> { ex.Message });
        }
    }
}