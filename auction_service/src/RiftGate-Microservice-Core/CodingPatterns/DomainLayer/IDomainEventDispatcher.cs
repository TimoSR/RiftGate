namespace CodingPatterns.DomainLayer;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync<T>(T entity) where T : AggregateRoot;
}