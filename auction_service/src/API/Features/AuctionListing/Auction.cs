using API.Features._shared.Domain;
using API.Features.AuctionListing.ValueObjects;
using CodingPatterns.DomainLayer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionListing;

public class Auction : Entity, IAggregateRoot
{
    [BsonId] [BsonRepresentation(BsonType.ObjectId)]  public override string Id { get; }
    
    public string SellerId { get; private set; }
    public Item Item { get; private set;}
    public Price Price { get; private set; }
    public AuctionLength AuctionLength { get; private set;}
    private DateTime StartTime { get; }
    public DateTime EndTime { get; private set; }
    public bool IsCompleted { get; private set; }
    
    public Auction(string sellerId, Item item, Price price, AuctionLength auctionLength)
    {
        SellerId = sellerId ?? throw new ArgumentNullException(nameof(sellerId));
        Item = item ?? throw new ArgumentNullException(nameof(item));
        Price = price ?? throw new ArgumentNullException(nameof(price));
        AuctionLength = auctionLength ?? throw new ArgumentNullException(nameof(auctionLength));
        StartTime =  DateTime.UtcNow;
        EndTime = StartTime.AddHours(auctionLength.Value);
        IsCompleted = false;
    }
    
    public void StartAuction()
    {
        // Validate conditions for starting the auction
        // Set StartTime and calculate EndTime
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