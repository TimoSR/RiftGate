using MediatR;
using x_serviceAPI.Features.AFeature.ApplicationLayer.IntegrationEvents.Published;
using x_serviceAPI.Features.AFeature.DomainLayer.DomainEvents;
using x_serviceAPI.Features.AFeature.InfrastructureLayer.Persistence.Google_PubSub;

// Domain Handler should handle the consequences of a single event. If new domain events are to be raised
//do not need to be explicitly called. Instead, they are automatically discovered and invoked by MediatR as long as they are registered in the application's dependency injection

namespace x_serviceAPI.Features.AFeature.ApplicationLayer.EventHandlers;

public class RegisterEventHandler : INotificationHandler<UserRegisteredDomainEvent>
{
    private readonly IntegrationEventHandler _integrationEventHandler; // Concrete implementation for publishing events

    public RegisterEventHandler(IntegrationEventHandler integrationEventHandler)
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


