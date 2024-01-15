using API.Features.AuctionOperations.Domain.Entities;
using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using CodingPatterns.InfrastructureLayer.IntegrationEvents.GooglePubSub._Attributes;
using ProtoBuf;

namespace API.Features.AuctionOperations.API.EventListeners.Subscriptions;

[ProtoContract]
public record AuctionSoldIntegrationEvent : ISubscribeEvent
{
    [ProtoMember(1)]
    public string AuctionId { get; init; }

    [ProtoMember(2)]
    public Bid WinningBid { get; init; }

    [ProtoMember(3)]
    public DateTime SellingTime { get; init; }
}