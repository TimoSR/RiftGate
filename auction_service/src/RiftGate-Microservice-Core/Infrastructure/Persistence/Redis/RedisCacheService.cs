using StackExchange.Redis;
using CodingPatterns.InfrastructureLayer;

namespace Infrastructure.Persistence.Redis;

public class RedisCacheService : ICache
{
    private readonly IConnectionMultiplexer _redisConnection;

    public RedisCacheService(IConnectionMultiplexer redisConnection)
    {
        _redisConnection = redisConnection ?? throw new ArgumentNullException(nameof(redisConnection));
    }

    public async Task<bool> SetValueAsync(string key, string value, TimeSpan? expiry = null)
    {
        var db = _redisConnection.GetDatabase();
        try
        {
            return await db.StringSetAsync(key, value, expiry);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error setting value in RedisCacheService for key: {Key}", key);
            return false;
        }
    }

    public async Task<(bool IsSuccess, string Value)> GetValueAsync(string key)
    {
        var db = _redisConnection.GetDatabase();
        try
        {
            var value = await db.StringGetAsync(key);
            return (value.HasValue, value);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error getting value from RedisCacheService for key: {Key}", key);
            return (false, null);
        }
    }

    public async Task<bool> RemoveValueAsync(string key)
    {
        var db = _redisConnection.GetDatabase();
        try
        {
            return await db.KeyDeleteAsync(key);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error removing key from RedisCacheService: {Key}", key);
            return false;
        }
    }

    public async Task<bool> KeyExistsAsync(string key)
    {
        var db = _redisConnection.GetDatabase();
        try
        {
            return await db.KeyExistsAsync(key);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error checking key existence in RedisCacheService: {Key}", key);
            return false;
        }
    }
}
