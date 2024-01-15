using API.Features.AuctionOperations.Domain.Events;
using MediatR;

namespace API.Features.AuctionOperations.Application.EventHandlers;

public class AuctionCompletedHandler : INotificationHandler<AuctionCompletedEvent>
{
    private readonly ILogger<AuctionCompletedHandler> _logger;

    public AuctionCompletedHandler(ILogger<AuctionCompletedHandler> logger)
    {
        _logger = logger;
    }
    
    public Task Handle(AuctionCompletedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(notification.Message);
        return Task.CompletedTask;
    }
}