using API.Features.AFeature.DomainLayer.Entities;

namespace API.Features.AFeature.DomainLayer.Repositories.ItemEntity;

public interface IItemQueryRepository
{
    Task<Item> GetByIdAsync(int itemId);
    Task<IEnumerable<Item>> GetAllAsync();
    Task<IEnumerable<Item>> GetByUserIdAsync(int userId); // If users can own items
}