using Infrastructure.Persistence._Interfaces;

namespace Infrastructure.Utilities.Containers;

public interface IServiceDependencies
{
    ICacheManager CacheManager { get; }
    IEventHandler EventHandler { get; }
}