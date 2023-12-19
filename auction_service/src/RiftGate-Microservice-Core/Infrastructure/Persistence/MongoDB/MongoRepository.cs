using CodingPatterns.DomainLayer;
using Infrastructure.Persistence._Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;

namespace Infrastructure.Persistence.MongoDB;

public abstract class MongoRepository<T> : IRepository<T> where T : Entity, IAggregateRoot
{
    protected string CollectionName => typeof(T).Name + "s";
    protected readonly IMongoDbManager _dbManager;
    protected readonly ILogger _logger;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    protected MongoRepository(IMongoDbManager dbManager, IDomainEventDispatcher domainEventDispatcher, ILogger logger)
    {
        _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
        _domainEventDispatcher = domainEventDispatcher ?? throw new ArgumentNullException(nameof(domainEventDispatcher));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected IMongoCollection<T> GetCollection() => _dbManager.GetCollection<T>(CollectionName);

    private FilterDefinition<T> IdFilter(string id) => Builders<T>.Filter.Eq("Id", id);

    public virtual async Task InsertAsync(T data)
    {
        try
        {
            var collection = GetCollection();
            await collection.InsertOneAsync(data);
            await _domainEventDispatcher.DispatchEventsAsync(data);
        }
        catch (MongoException ex)
        {
            throw new MongoRepositoryConnectionException($"Error connecting to MongoDB on insert operation. Details: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error inserting data into {CollectionName}. Details: {ex.Message}", ex);
        }
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        try
        {
            var collection = GetCollection();
            return await collection.Find(_ => true).ToListAsync();
        }
        catch (MongoException ex)
        {
            throw new MongoRepositoryConnectionException($"Error connecting to MongoDB on get all operation. Details: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error retrieving all data from {CollectionName}. Details: {ex.Message}", ex);
        }
    }

    public virtual async Task<T> GetByIdAsync(string id)
    {
        try
        {
            var collection = GetCollection();
            var result = await collection.Find(IdFilter(id)).FirstOrDefaultAsync();
            if (result == null)
            {
                throw new MongoRepositoryNotFoundException($"Entity with id {id} was not found in {CollectionName}.");
            }
            return result;
        }
        catch (MongoException ex)
        {
            throw new MongoRepositoryConnectionException($"Error connecting to MongoDB when retrieving entity with id {id}. Details: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error retrieving data by id {id} from {CollectionName}. Details: {ex.Message}", ex);
        }
    }

    public virtual async Task UpdateAsync(T updatedData)
    {
        try
        {
            var collection = GetCollection();
            var filter = IdFilter(updatedData.Id);
            var updateDefinition = CreateUpdateDefinition(updatedData);

            if (updateDefinition != null)
            {
                await collection.UpdateOneAsync(filter, updateDefinition);
            }
            await _domainEventDispatcher.DispatchEventsAsync(updatedData);
        }
        catch (MongoException ex)
        {
            throw new MongoRepositoryConnectionException($"Error connecting to MongoDB when updating entity with id {updatedData.Id}. Details: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error updating data by id {updatedData.Id} in {CollectionName}. Details: {ex.Message}", ex);
        }
    }

    private UpdateDefinition<T> CreateUpdateDefinition(T updatedData)
    {
        var updateProps = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);
        var updateDefinitionBuilder = Builders<T>.Update;
        UpdateDefinition<T> updateDefinition = null;

        foreach (var prop in updateProps)
        {
            if (Attribute.IsDefined(prop, typeof(BsonElementAttribute)))
            {
                var bsonElementAttribute = Attribute.GetCustomAttribute(prop, typeof(BsonElementAttribute)) as BsonElementAttribute;
                var propName = bsonElementAttribute.ElementName;
                var propValue = prop.GetValue(updatedData);
                var update = updateDefinitionBuilder.Set(propName, propValue);
                updateDefinition = updateDefinition == null ? update : Builders<T>.Update.Combine(updateDefinition, update);
            }
        }

        return updateDefinition;
    }

    public virtual async Task<bool> DeleteAsync(T deletedData)
    {
        try
        {
            var collection = GetCollection();
            var operationResult = await collection.DeleteOneAsync(IdFilter(deletedData.Id));
            var returnResult = operationResult.IsAcknowledged && operationResult.DeletedCount > 0;

            if (returnResult)
            {
                await _domainEventDispatcher.DispatchEventsAsync(deletedData);
            }

            return returnResult;
        }
        catch (MongoException ex)
        {
            throw new MongoRepositoryConnectionException($"Error connecting to MongoDB when deleting entity with id {deletedData.Id}. Details: {ex.Message}", ex);
        }
        catch (Exception ex)
        {
            throw new MongoRepositoryException($"Error deleting data by id {deletedData.Id} from {CollectionName}. Details: {ex.Message}", ex);
        }
    }
}
