using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using IntegrationEvents.Data;
using ProtoBuf;

namespace IntegrationEvents;

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