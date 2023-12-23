using CodingPatterns.DomainLayer;

namespace API.Features.AuctionAnalytics.Domain;

public class AuctionAnalytics : Entity, IAggregateRoot
{
    public string AuctionId { get; private set; }
    public string ItemId { get; private set; }
    public Bid FinalBid { get; private set; }
    public TimeSpan AuctionDuration { get; private set; }
    public string ItemCategory { get; private set; }

    public AuctionAnalytics(string auctionId, string itemId, Bid finalBid, TimeSpan auctionDuration, string itemCategory)
    {
        AuctionId = auctionId ?? throw new ArgumentNullException(nameof(auctionId));
        ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
        FinalBid = finalBid ?? throw new ArgumentNullException(nameof(finalBid));
        AuctionDuration = auctionDuration;
        ItemCategory = itemCategory ?? throw new ArgumentNullException(nameof(itemCategory));
    }
}