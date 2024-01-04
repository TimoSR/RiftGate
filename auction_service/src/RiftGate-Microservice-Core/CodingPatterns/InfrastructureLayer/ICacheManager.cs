namespace CodingPatterns.InfrastructureLayer;

public interface ICacheManager
{
    Task<T> GetAsync<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task InvalidateAsync(string key);

    // If you plan to use bulk operations in your application, consider adding these:
    Task<bool> SetValuesAsync<T>(Dictionary<string, T> keyValuePairs, TimeSpan? expiry = null);
    Task<Dictionary<string, T>> GetValuesAsync<T>(IEnumerable<string> keys);
    Task<bool> ExpireKeyAsync(string key, TimeSpan expiry);
}