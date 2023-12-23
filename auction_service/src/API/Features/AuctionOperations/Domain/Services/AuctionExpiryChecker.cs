using API.Features.AuctionOperations.Domain.Repositories;

namespace API.Features.AuctionOperations.Domain.Services;

public class AuctionExpiryChecker : IAuctionExpiryChecker
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly ITimeService _timeService;

    public AuctionExpiryChecker(IAuctionRepository auctionRepository, ITimeService timeService)
    {
        _auctionRepository = auctionRepository;
        _timeService = timeService;
    }

    public async Task CheckAndCompleteExpiredAuctions()
    {
        // Fetch all active auctions from the repository
        var activeAuctions = await _auctionRepository.GetAllActiveAuctionsAsync();

        foreach (var auction in activeAuctions)
        {
            // Directly call CheckAndCompleteAuction, which contains the expiry check
            auction.CheckAndCompleteAuction(_timeService);
            
            // Update the auction if it has been completed
            if (!auction.IsActive)
            {
                await _auctionRepository.UpdateAsync(auction);
            }
        }
    }
}