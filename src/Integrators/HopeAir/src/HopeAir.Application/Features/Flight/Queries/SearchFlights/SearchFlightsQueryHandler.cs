using FlightSearch.Core.Response;
using FluentValidation;
using HopeAir.Application.Extensions;
using HopeAir.Application.Interfaces;
using HopeAir.Domain.Models;
using MediatR;

namespace HopeAir.Application.Features.Flight.Queries.SearchFlights;

public class SearchFlightsQueryHandler : IRequestHandler<SearchFlightsQuery, ApiResponse<IEnumerable<SearchFlightsQueryResponse>>>
{
    private readonly ISoapClientService _soapClientService;
    private readonly IValidator<SearchFlightsQuery> _validator;


    public SearchFlightsQueryHandler(ISoapClientService soapClientService, IValidator<SearchFlightsQuery> validator)
    {
        _soapClientService = soapClientService;
        _validator = validator;
    }
    public async Task<ApiResponse<IEnumerable<SearchFlightsQueryResponse>>> Handle(SearchFlightsQuery request, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return ApiResponse<IEnumerable<SearchFlightsQueryResponse>>.ValidationError("Validation failed.", validationResult);
        }

        try
        {
            var soapRequest = SoapParseExtensions.CreateSoapEnvelope(new SoapSearchFlightRequest
                {
                    Date = request.DepartureDate,
                    Destination = request.Destination,
                    Origin = request.Origin,
                    PassengerCount = request.PassengerCount
                }, "GetFlightInfoRequest",
                "http://skyblue.com/flight");

            var soapFlightResponse = await _soapClientService.SendSoapRequestAsync(soapRequest);

            var flights = SoapParseExtensions.ParseSoapResponse(soapFlightResponse);
            
            if (!flights.Any())
            {
                return ApiResponse<IEnumerable<SearchFlightsQueryResponse>>.SuccessApiResponse(new List<SearchFlightsQueryResponse>(), "No flights found.", 200);
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
                    ProviderName = "HopeAir"
                })
                .OrderBy(f => f.Price)
                .ToList();
            
            return ApiResponse<IEnumerable<SearchFlightsQueryResponse>>.SuccessApiResponse(result, "Flights retrieved successfully.", 200);

            
        }
        catch (Exception ex)
        {
            //TODO: Log mekanizması eklenmelidir.
            return ApiResponse<IEnumerable<SearchFlightsQueryResponse>>.Error("An error occurred while fetching flights.", 500, new List<string> { ex.Message });

        }
    }
}