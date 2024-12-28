using AybJet.Application.Interfaces;
using AybJet.Domain.Models;
using Newtonsoft.Json;

namespace AybJet.Infrastructure.Services;

public class MockHttpClientService : IHttpClientService
{
    private readonly string _mockDataPath;

    public MockHttpClientService()
    {
        var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        _mockDataPath = Path.Combine(baseDirectory, "Resources", "mock-flight-data.json");
        // _mockDataPath = Path.Combine("/Users/eg/src/StudyCase/RoofStacks/FlightSearch/src/Integrators/AybJet/src/AybJet.Infrastructure/MockData/", "mock-flight-data.json");

    }

    public async Task<IEnumerable<T>> PostAsync<T>(string url, object body)
    {
        if (!File.Exists(_mockDataPath))
            throw new FileNotFoundException($"Mock data file not found at {_mockDataPath}");

        var jsonData = await File.ReadAllTextAsync(_mockDataPath);
        
        return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonData);
    }
}