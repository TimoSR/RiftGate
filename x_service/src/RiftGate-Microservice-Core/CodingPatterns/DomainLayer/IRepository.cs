namespace CodingPatterns.DomainLayer;

public interface IRepository<T> where T : IAggregateRoot
{
    Task InsertAsync(T entity);
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
    Task SoftDeleteAsync(T entity);
    Task DeleteAsync(T entity);
    Task UpdateAsync(T entity);
}