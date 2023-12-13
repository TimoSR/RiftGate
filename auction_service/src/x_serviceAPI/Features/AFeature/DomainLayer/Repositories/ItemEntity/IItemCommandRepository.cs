using Domain.Entities;

namespace Domain.Repositories.ItemEntity;

public interface IItemCommandRepository
{
    Task AddAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(int itemId);
}