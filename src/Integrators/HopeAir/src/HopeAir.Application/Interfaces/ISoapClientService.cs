using HopeAir.Application.Features.Flight.Queries.SearchFlights;
using HopeAir.Domain.Models;

namespace HopeAir.Application.Interfaces;

public interface ISoapClientService
{
    Task<string> SendSoapRequestAsync<T>(T query);
}