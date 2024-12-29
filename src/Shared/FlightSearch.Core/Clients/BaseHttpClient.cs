using System.Text.Json;
using Polly;
using Polly.Timeout;

namespace FlightSearch.Core.Clients;

public class BaseHttpClient
{
    private readonly HttpClient _httpClient;

    public BaseHttpClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }


    public async Task<T?> GetQueryAsync<T>(string uri, Dictionary<string, string>? queryParams = null)
    {
        return await SendRequestAsync<T>(HttpMethod.Get, uri, null, queryParams);
    }

    private async Task<T?> SendRequestAsync<T>(
        HttpMethod method,
        string baseUri,
        HttpContent? content = null,
        Dictionary<string, string>? queryParams = null)
    {
        try
        {
            if (queryParams != null && queryParams.Any())
            {
                var queryString = string.Join("&", queryParams.Select(kvp =>
                    $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));
                baseUri = $"{baseUri}?{queryString}";
            }
            
            using var request = new HttpRequestMessage(method, baseUri)
            {
                Content = content
            };
            
            var response = await _httpClient.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Request to {baseUri} failed with status code {response.StatusCode}");
                return default;
            }
            
            var responseData = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(responseData, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred while sending request to {baseUri}: {ex.Message}");
            return default;
        }
    }
}