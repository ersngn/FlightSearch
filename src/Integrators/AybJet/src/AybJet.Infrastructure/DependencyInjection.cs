using AybJet.Application.Interfaces;
using AybJet.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AybJet.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IHttpClientService, MockHttpClientService>();

        return services;
    }
}