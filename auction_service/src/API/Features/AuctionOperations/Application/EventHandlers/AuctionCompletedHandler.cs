using API.Features.AuctionOperations.Application.EventHandlers.Topics;
using API.Features.AuctionOperations.Domain.Events;
using Infrastructure.Persistence._Interfaces;
using MediatR;

namespace API.Features.AuctionOperations.Application.EventHandlers;

public class AuctionCompletedHandler : INotificationHandler<AuctionCompletedEvent>
{
    private readonly ILogger<AuctionCompletedHandler> _logger;
    private readonly IIntegrationEventHandler _eventHandler;

    public AuctionCompletedHandler(
        ILogger<AuctionCompletedHandler> logger,
        IIntegrationEventHandler eventHandler)
    {
        _logger = logger;
        _eventHandler = eventHandler;
    }
    
    public async Task Handle(AuctionCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(notification.Message);
        
        var integrationEvent = new AuctionCompletedIntEvent
        {
            AuctionId = notification.AuctionId,
            CompletionTime = notification.CompletionTime
        };

        await _eventHandler.PublishProtobufEventAsync(integrationEvent);
    }
}