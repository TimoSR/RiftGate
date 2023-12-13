using Infrastructure.Persistence._Interfaces;

namespace Infrastructure.UtilityServices.Containers;

public class ServiceDependencies : IServiceDependencies
{
    public IIntegrationEventHandler IntegrationEventHandler { get; }
    public ICacheManager CacheManager { get; }
    
    public ServiceDependencies(
        IIntegrationEventHandler integrationEventHandler = null,
        ICacheManager cacheManager = null
    )
    {
        IntegrationEventHandler = integrationEventHandler;
        CacheManager = cacheManager;
    }
}