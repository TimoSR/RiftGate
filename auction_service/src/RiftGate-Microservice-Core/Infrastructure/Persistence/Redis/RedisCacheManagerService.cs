using StackExchange.Redis;
using CodingPatterns.InfrastructureLayer;
using Newtonsoft.Json;

namespace Infrastructure.Persistence.Redis;

public class RedisCacheManagerService : ICacheManager
{
    private readonly IDatabase _database;

    public RedisCacheManagerService(IConnectionMultiplexer redisConnection)
    {
        _database = redisConnection.GetDatabase() ?? throw new ArgumentNullException(nameof(redisConnection));
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _database.StringGetAsync(key);
        return value.IsNullOrEmpty ? default : JsonConvert.DeserializeObject<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var serializedValue = JsonConvert.SerializeObject(value);
        await _database.StringSetAsync(key, serializedValue, expiry);
    }

    public async Task InvalidateAsync(string key)
    {
        await _database.KeyDeleteAsync(key);
    }

    public async Task<bool> KeyExistsAsync(string key)
    {
        return await _database.KeyExistsAsync(key);
    }
    
    public async Task<bool> SetValuesAsync<T>(Dictionary<string, T> keyValuePairs, TimeSpan? expiry = null)
    {
        var tasks = new List<Task>();
        foreach (var pair in keyValuePairs)
        {
            var serializedValue = JsonConvert.SerializeObject(pair.Value);
            tasks.Add(_database.StringSetAsync(pair.Key, serializedValue, expiry));
        }
        await Task.WhenAll(tasks);
        return true;
    }

    public async Task<Dictionary<string, T>> GetValuesAsync<T>(IEnumerable<string> keys)
    {
        var results = new Dictionary<string, T>();
        RedisKey[] redisKeys = Array.ConvertAll(keys.ToArray(), item => (RedisKey)item);
        RedisValue[] redisValues = await _database.StringGetAsync(redisKeys);

        for (int i = 0; i < redisKeys.Length; i++)
        {
            if (redisValues[i].HasValue)
            {
                results.Add(redisKeys[i], JsonConvert.DeserializeObject<T>(redisValues[i]));
            }
        }
        return results;
    }

    public async Task<bool> ExpireKeyAsync(string key, TimeSpan expiry)
    {
        return await _database.KeyExpireAsync(key, expiry);
    }
}
