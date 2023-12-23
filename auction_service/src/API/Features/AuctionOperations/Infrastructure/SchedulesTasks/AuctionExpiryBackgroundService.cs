using API.Features.AuctionOperations.Domain.Services;

namespace API.Features.AuctionOperations.Infrastructure.SchedulesTasks;

public class AuctionExpiryBackgroundService : BackgroundService
{
    private readonly IAuctionExpiryChecker _auctionExpiryChecker;

    public AuctionExpiryBackgroundService(IAuctionExpiryChecker auctionExpiryChecker)
    {
        _auctionExpiryChecker = auctionExpiryChecker;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _auctionExpiryChecker.CheckAndCompleteExpiredAuctions();
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Adjust the time interval as needed
        }
    }
}