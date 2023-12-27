using API.Features.AuctionOperations.Domain.Events;
using MediatR;

namespace API.Features.AuctionOperations.Application.EventHandlers;

public class AuctionEventHandler : 
    INotificationHandler<AuctionStartedEvent>,
    INotificationHandler<AuctionCompletedEvent>,
    INotificationHandler<BidPlacedEvent>
{
    private readonly ILogger<AuctionEventHandler> _logger;

    public AuctionEventHandler(ILogger<AuctionEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(AuctionStartedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(notification.Message);
        return Task.CompletedTask;
    }

    public Task Handle(AuctionCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(notification.Message);
        return Task.CompletedTask;
    }

    public Task Handle(BidPlacedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(notification.Message);
        return Task.CompletedTask;
    }
}