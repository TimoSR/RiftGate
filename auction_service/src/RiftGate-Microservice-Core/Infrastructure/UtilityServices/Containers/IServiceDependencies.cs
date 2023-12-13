using Infrastructure.Persistence._Interfaces;

namespace Infrastructure.UtilityServices.Containers;

public interface IServiceDependencies
{
    ICacheManager CacheManager { get; }
    IIntegrationEventHandler IntegrationEventHandler { get; }
}