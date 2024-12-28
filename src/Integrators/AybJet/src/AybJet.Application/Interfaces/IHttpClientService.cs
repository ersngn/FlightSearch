using AybJet.Domain.Models;

namespace AybJet.Application.Interfaces;

public interface IHttpClientService
{
    Task<IEnumerable<T>> PostAsync<T>(string url, object body);
}