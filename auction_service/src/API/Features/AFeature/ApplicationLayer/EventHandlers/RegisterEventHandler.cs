using API.Features.AFeature.ApplicationLayer.IntegrationEvents.Published;
using API.Features.AFeature.DomainLayer.DomainEvents;
using Infrastructure.Persistence._Interfaces;
using MediatR;

namespace API.Features.AFeature.ApplicationLayer.EventHandlers;

public class RegisterEventHandler : INotificationHandler<UserRegisteredDomainEvent>
{
    private readonly IIntegrationEventHandler _integrationEventHandler; // Concrete implementation for publishing events

    // Domain Handler should handle the consequences of a single event. If new domain events are to be raised
    //do not need to be explicitly called. Instead, they are automatically discovered and invoked by MediatR as long as they are registered in the application's dependency injection
    
    public RegisterEventHandler(IIntegrationEventHandler integrationEventHandler)
    {
        _integrationEventHandler = integrationEventHandler;
    }
    
    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        // Side effect within the same bounded context
        // Like sending a email if successful 

        // Translate and publish an integration event using EventHandler
        var integrationEvent = new UserRegisteredIntegrationEvent(notification.UserId, notification.Username, notification.Email);
        await _integrationEventHandler.PublishProtobufEventAsync(integrationEvent); // or PublishProtobufEventAsync, based on your needs
    }
}


