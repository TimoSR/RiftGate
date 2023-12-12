using Domain.DomainEvents;
using Infrastructure.Persistence.Google_PubSub;
using MediatR;

// Domain Handler should handle the consequences of a single event. If new domain events are to be raised
//do not need to be explicitly called. Instead, they are automatically discovered and invoked by MediatR as long as they are registered in the application's dependency injection

namespace Application.AppServices.V1.xService.EventHandlers;

public class RegisterEventHandler : INotificationHandler<UserRegisteredEvent>
{
    private readonly IEmailService _emailService;
    private readonly IntegrationEventHandler _integrationEventHandler; // Concrete implementation for publishing events

    public RegisterEventHandler(IEmailService emailService, IntegrationEventHandler integrationEventHandler)
    {
        _emailService = emailService;
        _integrationEventHandler = integrationEventHandler;
    }
    
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        // Side effect within the same bounded context
        await _emailService.SendConfirmationEmail(notification.Email);

        // Translate and publish an integration event using EventHandler
        var integrationEvent = new UserRegisteredEvent(notification.UserId, notification.Username, notification.Email);
        
        await _integrationEventHandler.PublishProtobufEventAsync(integrationEvent); // or PublishProtobufEventAsync, based on your needs
    }
}


