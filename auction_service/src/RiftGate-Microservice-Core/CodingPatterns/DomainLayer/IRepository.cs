namespace CodingPatterns.DomainLayer;

public interface IRepository<T> where T : IAggregateRoot
{
    Task AddAsync(T entity);
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task SoftDeleteAsync(T entity);
    Task DeleteAsync(T entity);
    Task UpdateAsync(T entity);
}