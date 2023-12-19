namespace Infrastructure.Persistence.MongoDB;

public interface IRepository<T>
{
    Task InsertAsync(T data);
    Task<List<T>> GetAllAsync();
    Task<T> GetByIdAsync(string id);
    Task<bool> DeleteAsync(T deletedData);
    Task UpdateAsync(T updatedData);
}