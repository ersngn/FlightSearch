using FlightSearch.Application.Feature.Flights.Queries.SearchFlights;
using FlightSearch.Application.Interfaces;
using FlightSearch.Core.Response;
using FlightSearch.Domain.Model;

namespace FlightSearch.Infrastructure.Services;

public class ProviderService : IProviderService
{
    private readonly IEnumerable<ISearchProvider> _searchProviders;

    public ProviderService(IEnumerable<ISearchProvider> searchProviders)
    {
        _searchProviders = searchProviders;
    }

    public async Task<IEnumerable<Flight>> SearchFlightsAsync(SearchFlightsQuery query)
    {
        var tasks = _searchProviders.Select(async provider =>
        {
            try
            {
                return await provider.SearchFlightsAsync(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in provider {provider.ProviderName}: {ex.Message}");
                return ApiResponse<IEnumerable<Flight>>.SuccessApiResponse(Enumerable.Empty<Flight>());
            }
        });

        var results = await Task.WhenAll(tasks);
        
        return results
            .Where(result => result != null && result.Data != null)
            .SelectMany(result => result.Data)
            .OrderBy(f => f.Price)
            .ToList();
    }
}