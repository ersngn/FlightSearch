using HopeAir.Application.Interfaces;
using HopeAir.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HopeAir.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<ISoapClientService, MockSoapClientService>();
        return services;
    }
}