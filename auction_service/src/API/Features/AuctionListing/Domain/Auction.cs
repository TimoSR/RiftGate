using API.Features.AuctionListing.Domain.ValueObjects;
using CodingPatterns.DomainLayer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionListing.Domain;

public class Auction : Entity, IAggregateRoot
{
    [BsonId] [BsonRepresentation(BsonType.ObjectId)]  public override string Id { get; }
    
    public string SellerId { get; private set; }
    public Item Item { get; private set;}
    public Price AskPrice { get; private set; }
    private AuctionLength AuctionLength { get; }
    private DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public bool IsCompleted { get; private set; } = false;
    public Bid HighestBid { get; }
    
    public Auction(string sellerId, Item item, Price price, AuctionLength auctionLength)
    {
        SellerId = sellerId ?? throw new ArgumentNullException(nameof(sellerId));
        Item = item ?? throw new ArgumentNullException(nameof(item));
        AskPrice = price ?? throw new ArgumentNullException(nameof(price));
        AuctionLength = auctionLength ?? throw new ArgumentNullException(nameof(auctionLength));
    }
    
    public void StartAuction()
    {
        // Set StartTime and calculate EndTime
        StartTime =  DateTime.UtcNow;
        EndTime = StartTime.AddHours(AuctionLength.Value);
        // Possibly raise an event indicating the auction has started
    }
    
    public void EndAuction()
    {
        // Check if the auction is already completed
        // Validate conditions for ending the auction
        // Determine the auction result (e.g., winning bid, if applicable)
        // Mark the auction as completed
        // Possibly raise an event indicating the auction has ended
    }
    
    public void CancelAuction()
    {
        // Validate conditions for cancellation (e.g., no bids placed)
        // Perform cancellation logic
        // Mark the auction as cancelled
        // Possibly raise an event indicating the auction has been cancelled
    }
    
    public void PlaceBid(Bid bid)
    {
        // Ensure the auction is active
        // Validate the bid (e.g., bid amount, bidder eligibility)
        // Add the bid to the auction
        // Update the state as necessary
        // Possibly raise an event for a new bid being placed
    }
    
    public Bid GetCurrentHighestBid()
    {
        // Return the current highest bid
    }

    public IEnumerable<Bid> GetAllBids()
    {
        // Return all bids placed in this auction
    }

}