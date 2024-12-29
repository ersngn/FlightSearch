namespace FlightSearch.Infrastructure.Extesions;

public static class DictionaryExtensions
{
    public static Dictionary<string, string> ToQueryParameters<T>(T obj)
    {
        return typeof(T).GetProperties()
            .Where(p => p.GetValue(obj) != null) 
            .ToDictionary(
                prop => prop.Name,
                prop => prop.GetValue(obj)!.ToString()!
            );
    }
}