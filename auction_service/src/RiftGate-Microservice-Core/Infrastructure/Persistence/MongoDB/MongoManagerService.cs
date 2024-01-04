using Infrastructure.Persistence._Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Persistence.MongoDB;

public class MongoManagerService : IMongoDbManager
{
    private readonly IMongoClient _client;
    private readonly string _defaultDatabase;
    private readonly IDictionary<string, string> _databaseNames;

    public MongoManagerService(IMongoClient client, string defaultDatabase, IDictionary<string, string> databaseNames)
    {
        _client = client;
        _defaultDatabase = defaultDatabase; 
        _databaseNames = databaseNames;
    }

    public IMongoClient GetClient()
    {
        return _client;
    }
    
    public IMongoDatabase GetDatabase() {
        return _client.GetDatabase(_defaultDatabase);
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _client.GetDatabase(_defaultDatabase).GetCollection<T>(collectionName);
    }

    public IMongoDatabase SelectFromDatabases(string key)
    {
        if (!_databaseNames.TryGetValue(key, out var databaseName))
        {
            throw new KeyNotFoundException($"No database configuration found for key: {key}");
        }
        return _client.GetDatabase(databaseName);
    }
}