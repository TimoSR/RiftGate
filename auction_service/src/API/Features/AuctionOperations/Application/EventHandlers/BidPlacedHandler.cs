using API.Features.AuctionOperations.Domain.Events;
using MediatR;

namespace API.Features.AuctionOperations.Application.EventHandlers;

public class BidPlacedHandler : INotificationHandler<BidPlacedEvent>
{
    private readonly ILogger<BidPlacedHandler> _logger;

    public BidPlacedHandler(ILogger<BidPlacedHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(BidPlacedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(notification.Message);
        return Task.CompletedTask;
    }
}