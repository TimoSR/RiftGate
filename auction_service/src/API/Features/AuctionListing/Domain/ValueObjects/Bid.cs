namespace API.Features.AuctionListing.Domain.ValueObjects;

public record Bid
{
    public decimal Amount { get; }
    public string BidderId { get; }
    public DateTime TimeStamp { get; }

    public Bid(decimal amount, string bidderId)
    {
        if (amount <= 0) throw new ArgumentException("Amount cannot be negative or zero.");

        if (string.IsNullOrWhiteSpace(BidderId)) throw new ArgumentNullException(nameof(BidderId));
        
        Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
        BidderId = bidderId;
        TimeStamp = DateTime.UtcNow;
    }
}