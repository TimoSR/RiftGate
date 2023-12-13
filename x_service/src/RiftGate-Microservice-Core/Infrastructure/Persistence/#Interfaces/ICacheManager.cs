namespace Infrastructure.Persistence._Interfaces;

public interface ICacheManager
{
    Task<bool> SetValueAsync(string key, string value, TimeSpan? expiry = null);
    Task<(bool IsSuccess, string Value)> GetValueAsync(string key);
    Task<bool> RemoveValueAsync(string key);
    Task<bool> KeyExistsAsync(string key);
}