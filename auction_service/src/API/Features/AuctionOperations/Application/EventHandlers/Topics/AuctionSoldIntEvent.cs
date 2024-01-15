using API.Features.AuctionOperations.Domain.Entities;
using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using CodingPatterns.InfrastructureLayer.IntegrationEvents.GooglePubSub._Attributes;
using ProtoBuf;

namespace API.Features.AuctionOperations.Application.EventHandlers.Topics;

[ProtoContract]
[TopicName("AuctionSoldTopic")]
public readonly record struct AuctionSoldIntEvent : IPublishEvent
{
    [ProtoMember(1)]
    public string AuctionId { get; init; }

    [ProtoMember(2)]
    public Bid WinningBid { get; init; }

    [ProtoMember(3)]
    public DateTime SellingTime { get; init; }

    public AuctionSoldIntEvent(string auctionId, Bid winningBid, DateTime sellingTime)
    {
        AuctionId = auctionId;
        WinningBid = winningBid;
        SellingTime = sellingTime;
    }

    public string Message => 
        $"Auction '{AuctionId}' was successfully sold to bidder '{WinningBid.BidderId}' " +
        $"with a winning bid of {WinningBid.BidAmount} at {SellingTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}