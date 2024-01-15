using API.Features.AuctionOperations.Domain.Events;
using MediatR;

namespace API.Features.AuctionOperations.Application.EventHandlers;

public class AuctionStartedHandler : INotificationHandler<AuctionStartedEvent>
{
    private readonly ILogger<AuctionStartedHandler> _logger;

    public AuctionStartedHandler(ILogger<AuctionStartedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(AuctionStartedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation(notification.Message);
        return Task.CompletedTask;
    }
}