using System.Xml;
using HopeAir.Application.Features.Flight.Queries.SearchFlights;
using HopeAir.Application.Interfaces;

namespace HopeAir.Infrastructure.Services;

public class MockSoapClientService : ISoapClientService
{
    private readonly string _mockDataPath;

    public MockSoapClientService()
    {
        _mockDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "hope-air-mock-flight-data.xml");
    }
    
    public async Task<string> SendSoapRequestAsync<T>(T query)
    {
        return await File.ReadAllTextAsync(_mockDataPath);
    }
}