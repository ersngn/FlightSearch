using FlightSearch.Application.Interfaces;
using FlightSearch.Core.Clients;
using FlightSearch.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FlightSearch.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddHttpClient<BaseHttpClient>((serviceProvider, client) =>
        {
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
        services.AddSingleton<ISearchProvider, HopeAirSearchProvider>();
        services.AddSingleton<ISearchProvider, AybJetSearchProvider>();
        services.AddSingleton<IProviderService, ProviderService>();

        return services;
    }
}