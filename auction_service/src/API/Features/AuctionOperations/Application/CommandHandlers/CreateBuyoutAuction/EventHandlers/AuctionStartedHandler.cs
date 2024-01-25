using API.Features.AuctionOperations.Application.CommandHandlers.CreateBuyoutAuction.Topics;
using API.Features.AuctionOperations.Domain.Events;
using Infrastructure.Persistence._Interfaces;
using MediatR;

namespace API.Features.AuctionOperations.Application.CommandHandlers.CreateBuyoutAuction.EventHandlers;

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
        
        var integrationEvent = new AuctionStartedIntegrationEvent(
            auctionId : notification.AuctionId,
            startTime : notification.StartTime
        );

        await _eventHandler.PublishProtobufEventAsync(integrationEvent);
        
        // var result = await _eventHandler.PublishProtobufEventAsync(integrationEvent);
        //
        // _logger.LogInformation(result);
    }
}