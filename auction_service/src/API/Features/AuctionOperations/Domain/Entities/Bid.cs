using API.Features.AuctionOperations.Domain.Services;
using API.Features.AuctionOperations.Domain.ValueObjects;
using CodingPatterns.DomainLayer;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace API.Features.AuctionOperations.Domain.Entities;

[ProtoContract]
public class Bid : Entity
{
    [BsonElement("bidAmount")]
    [ProtoMember(1)]
    public Price BidAmount { get; private set; }

    [BsonElement("bidderId")]
    [ProtoMember(2)]
    public string BidderId { get; private set; }

    [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    [BsonElement("timeStamp")]
    [ProtoMember(3)]
    public DateTime TimeStamp { get; private set; }

    public Bid(string bidderId, Price bidAmount, ITimeService timeService)
    {
        Validate(bidderId, bidAmount);
        BidderId = bidderId;
        BidAmount = bidAmount;
        TimeStamp = timeService.GetCurrentTime();
    }

    private static void Validate(string bidderId, Price bidAmount)
    {
        if (string.IsNullOrWhiteSpace(bidderId)) 
            throw new ArgumentException("Bidder ID cannot be null or whitespace.", nameof(bidderId));

        if (bidAmount == null) 
            throw new ArgumentNullException(nameof(bidAmount), "Bid amount cannot be null.");
    }
}