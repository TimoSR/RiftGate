namespace CodingPatterns.DomainLayer;

public interface IRepository<T> where T : IAggregateRoot
{
    Task InsertAsync(T data);
    Task<List<T>> GetAllAsync();
    Task<T?> GetByIdAsync(string id);
    Task<bool> DeleteAsync(T deletedData);
    Task UpdateAsync(T updatedData);
}