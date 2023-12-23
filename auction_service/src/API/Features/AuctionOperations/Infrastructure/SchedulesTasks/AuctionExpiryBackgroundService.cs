using API.Features.AuctionOperations.Domain.Services;

namespace API.Features.AuctionOperations.Infrastructure.SchedulesTasks;

using Microsoft.Extensions.DependencyInjection;

public class AuctionExpiryBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public AuctionExpiryBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var auctionExpiryChecker = scope.ServiceProvider.GetRequiredService<IAuctionExpiryChecker>();
                await auctionExpiryChecker.CheckAndCompleteExpiredAuctions();
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Adjust the time interval as needed
        }
    }
}
