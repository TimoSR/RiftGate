using Infrastructure.Persistence._Interfaces;
using StackExchange.Redis;
using Microsoft.Extensions.Logging;
using System;

namespace Infrastructure.Persistence.Redis;

public class RedisManager : ICacheManager
{
    private readonly IConnectionMultiplexer _redisConnection;
    private readonly ILogger<RedisManager> _logger;

    public RedisManager(IConnectionMultiplexer redisConnection, ILogger<RedisManager> logger)
    {
        _redisConnection = redisConnection;
        _logger = logger;
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
            _logger.LogError(ex, "Error setting value in Redis for key: {Key}", key);
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
            _logger.LogError(ex, "Error getting value from Redis for key: {Key}", key);
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
            _logger.LogError(ex, "Error removing key from Redis: {Key}", key);
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
            _logger.LogError(ex, "Error checking key existence in Redis: {Key}", key);
            return false;
        }
    }
}
