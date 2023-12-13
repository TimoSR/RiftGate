using API.Features.AFeature.DomainLayer.Entities;

namespace API.Features.AFeature.DomainLayer.Repositories.ItemEntity;

public interface IItemCommandRepository
{
    Task AddAsync(Item item);
    Task UpdateAsync(Item item);
    Task DeleteAsync(int itemId);
}