using CodingPatterns.DomainLayer;
using MediatR;

namespace Infrastructure.Persistence.EventHandlers;

// Should be used within the repository dispatching the events after persistence (saved). 

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublisher _mediator;

    public DomainEventDispatcher(IPublisher mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchEventsAsync<T>(T entity) where T : AggregateRoot
    {
        var domainEvents = entity.DomainEvents?.ToList() ?? new List<INotification>();

        foreach (var domainEvent in domainEvents)
        {
            await _mediator.Publish(domainEvent);
        }

        entity.ClearDomainEvents();
    }
}
