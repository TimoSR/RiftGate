using CodingPatterns.DomainLayer;
using CodingPatterns.InfrastructureLayer;

namespace Infrastructure.Persistence.Redis;

public class CachedRepositoryDecorator<T>: IRepository<T> where T : IAggregateRoot
{
    private readonly IRepository<T> _repository;
    private readonly ICacheManager _cacheManager;

    protected CachedRepositoryDecorator(IRepository<T> repository, ICacheManager cacheManager)
    {
        _repository = repository;
        _cacheManager = cacheManager;
    }

    public Task<List<T>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<T> GetByIdAsync(string id)
    {
        var cacheKey = $"{typeof(T).Name}-{id}";
        var cachedEntity = await _cacheManager.GetAsync<T>(cacheKey);
        if (cachedEntity != null)
        {
            return cachedEntity;
        }

        var entity = await _repository.GetByIdAsync(id);
        if (entity != null)
        {
            await _cacheManager.SetAsync(cacheKey, entity, TimeSpan.FromMinutes(30));
        }
        return entity;
    }

    public Task SoftDeleteAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(T entity)
    {
        await _repository.AddAsync(entity);
    }

    public async Task UpdateAsync(T entity)
    {
        await _repository.UpdateAsync(entity);
        var cacheKey = $"{typeof(T).Name}-{entity.Id}";
        await _cacheManager.InvalidateAsync(cacheKey);
    }

    public async Task DeleteAsync(T entity)
    {
        await _repository.DeleteAsync(entity);
        var cacheKey = $"{typeof(T).Name}-{entity.Id}";
        await _cacheManager.InvalidateAsync(cacheKey);
    }
}
