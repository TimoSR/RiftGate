using API.Features.AuctionOperations.Application.EventHandlers.Topics;
using API.Features.AuctionOperations.Domain.Events;
using Infrastructure.Persistence._Interfaces;
using MediatR;

namespace API.Features.AuctionOperations.Application.EventHandlers;

public class AuctionStartedHandler : INotificationHandler<AuctionStartedEvent>
{
    private readonly ILogger<AuctionStartedHandler> _logger;
    private readonly IIntegrationEventHandler _eventHandler;

    public AuctionStartedHandler(
        ILogger<AuctionStartedHandler> logger,
        IIntegrationEventHandler eventHandler)
    {
        _logger = logger;
        _eventHandler = eventHandler;
    }

    public async Task Handle(AuctionStartedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(notification.Message);
        
        var integrationEvent = new AuctionStartedIntEvent
        {
            AuctionId = notification.AuctionId,
            StartTime = notification.StartTime
        };

        await _eventHandler.PublishProtobufEventAsync(integrationEvent);
    }
}