using CodingPatterns.DomainLayer;

namespace CodingPatterns.InfrastructureLayer;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync<T>(T entity) where T : AggregateRoot;
}