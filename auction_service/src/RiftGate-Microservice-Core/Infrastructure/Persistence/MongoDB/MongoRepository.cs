using CodingPatterns.DomainLayer;
using Infrastructure.Persistence._Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Infrastructure.Persistence.MongoDB;

public abstract class MongoRepository<T> : IRepository<T> where T : Entity, IAggregateRoot
{
    protected virtual string CollectionName => typeof(T).Name + "s";

    protected readonly IMongoDbManager _dbManager;
    protected readonly ILogger _logger;
    private readonly IDomainEventDispatcher _domainEventDispatcher;

    protected MongoRepository(IMongoDbManager dbManager, IDomainEventDispatcher domainEventDispatcher, ILogger logger)
    {
        _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager));
        _domainEventDispatcher = domainEventDispatcher;
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
        catch (Exception ex)
        {
            _logger.LogError($"Error inserting data into {CollectionName}: {ex.Message}");
            throw;
        }
    }

    public virtual async Task<List<T>> GetAllAsync()
    {
        try
        {
            var collection = GetCollection();
            return await collection.Find(_ => true).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving all data from {CollectionName}: {ex.Message}");
            throw;
        }
    }

    public virtual async Task<T> GetByIdAsync(string id)
    {
        try
        {
            var collection = GetCollection();
            return await collection.Find(IdFilter(id)).FirstOrDefaultAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error retrieving data by id {id} from {CollectionName}: {ex.Message}");
            throw;
        }
    }

    public virtual async Task UpdateAsync(T updatedData)
    {
        try
        {
            var collection = GetCollection();
            var filter = IdFilter(updatedData.Id);
            UpdateDefinition<T> updateDefinition = CreateUpdateDefinition(updatedData);

            if (updateDefinition != null)
            {
                await collection.UpdateOneAsync(filter, updateDefinition);
            }
            await _domainEventDispatcher.DispatchEventsAsync(updatedData);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error updating data by id {updatedData.Id} in {CollectionName}: {ex.Message}");
            throw;
        }
    }

    private UpdateDefinition<T> CreateUpdateDefinition(T updatedData)
    {
        var updateProps = typeof(T).GetProperties();
        var updateDefinitionBuilder = Builders<T>.Update;
        UpdateDefinition<T> updateDefinition = null;

        foreach (var prop in updateProps)
        {
            var propValue = prop.GetValue(updatedData);
            var update = updateDefinitionBuilder.Set(prop.Name, propValue);

            updateDefinition = updateDefinition == null
                               ? update
                               : Builders<T>.Update.Combine(updateDefinition, update);
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
            
            if (!returnResult)
            {
                return returnResult;
            }
            
            await _domainEventDispatcher.DispatchEventsAsync(deletedData);

            return returnResult;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error deleting data by id {deletedData.Id} from {CollectionName}: {ex.Message}");
            throw;
        }
    }
}