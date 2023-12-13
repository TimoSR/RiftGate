using MongoDB.Driver;

namespace Infrastructure.Persistence._Interfaces;

public interface IMongoDbManager
{
    IMongoClient GetClient();
    IMongoDatabase GetDatabase();
    IMongoCollection<T> GetCollection<T>(string collectionName);
}