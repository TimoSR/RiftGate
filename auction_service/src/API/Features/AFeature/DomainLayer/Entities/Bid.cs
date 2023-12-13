namespace API.Features.AFeature.DomainLayer.Entities;

public class Bid
{
    public int BidId { get; set; }
    public int UserId { get; set; }
    public int AuctionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BidTime { get; set; }

    public virtual User User { get; set; } // Navigation property
    public virtual Auction Auction { get; set; } // Navigation property
}