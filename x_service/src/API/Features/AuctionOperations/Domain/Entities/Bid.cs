using API.Features.AuctionOperations.Domain.ValueObjects;
using CodingPatterns.DomainLayer;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionOperations.Domain.Entities;

public class Bid : Entity
{
    [BsonElement("bidAmount")]
    public Price BidAmount { get; }

    [BsonElement("bidderId")]
    public string BidderId { get; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement("timeStamp")]
    public DateTime TimeStamp { get; }

    public Bid(string bidderId, Price bidAmount, DateTime timeStamp)
    {
        Validate(bidderId, bidAmount);
        BidderId = bidderId;
        BidAmount = bidAmount;
        TimeStamp = timeStamp;
    }

    private static void Validate(string bidderId, Price bidAmount)
    {
        if (string.IsNullOrWhiteSpace(bidderId)) 
            throw new ArgumentException("Bidder ID cannot be null or whitespace.", nameof(bidderId));

        if (bidAmount == null) 
            throw new ArgumentNullException(nameof(bidAmount), "Bid amount cannot be null.");
    }
}