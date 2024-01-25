using API.Features.AuctionOperations.Domain.Entities;
using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using CodingPatterns.InfrastructureLayer.IntegrationEvents.GooglePubSub._Attributes;
using ProtoBuf;

namespace API.Features.AuctionOperations.Application.CommandHandlers.PlaceBid.Topics;

[ProtoContract]
[TopicName("BidPlacedTopic")]
public record BidPlacedIntegrationEvent : IPublishEvent
{
    [ProtoMember(1)]
    public string AuctionId { get; init; }

    [ProtoMember(2)]
    public Bid Bid { get; init; }

    public BidPlacedIntegrationEvent(string auctionId, Bid bid)
    {
        AuctionId = auctionId;
        Bid = bid;
    }

    public string Message => 
        $"Event: New bid placed on auction {AuctionId}. " +
        $"Timestamp: {Bid.TimeStamp:yyyy-MM-dd HH:mm:ss} (UTC). " +
        $"Bid Amount: {Bid.BidAmount}. " +
        $"Bidder: {Bid.BidderId}.";
}