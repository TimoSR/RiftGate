using Infrastructure.Persistence._Interfaces;
using Infrastructure.Utilities._Interfaces;

namespace Infrastructure.Utilities.Containers;

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