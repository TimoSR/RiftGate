using Infrastructure.Persistence._Interfaces;
using Infrastructure.Utilities._Interfaces;

namespace Infrastructure.Utilities.Containers;

public class ServiceDependencies : IServiceDependencies
{
    public IEventHandler EventHandler { get; }
    public ICacheManager CacheManager { get; }
    
    public ServiceDependencies(
        IEventHandler eventHandler = null,
        ICacheManager cacheManager = null
    )
    {
        EventHandler = eventHandler;
        CacheManager = cacheManager;
    }
}