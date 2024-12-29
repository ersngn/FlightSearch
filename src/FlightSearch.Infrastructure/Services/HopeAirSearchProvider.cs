using FlightSearch.Application.Feature.Flights.Queries.SearchFlights;
using FlightSearch.Application.Interfaces;
using FlightSearch.Core.Clients;
using FlightSearch.Core.Response;
using FlightSearch.Domain.Constants;
using FlightSearch.Domain.Model;
using FlightSearch.Infrastructure.Extesions;

namespace FlightSearch.Infrastructure.Services;

public class HopeAirSearchProvider : ISearchProvider
{
    private readonly BaseHttpClient _baseHttpClient;

    public string ProviderName => "HopeAir";

    public HopeAirSearchProvider(BaseHttpClient baseHttpClient)
    {
        _baseHttpClient = baseHttpClient;
    }

    public async Task<ApiResponse<IEnumerable<Flight>>?> SearchFlightsAsync(SearchFlightsQuery query)
    {
        var queryParams = DictionaryExtensions.ToQueryParameters(query);

        var result = await _baseHttpClient.GetQueryAsync<ApiResponse<IEnumerable<Flight>>>(
            UriConstants.HopeAirUrls.HopeAirFlightSearchUri,
            queryParams);

        return result;
    }
}