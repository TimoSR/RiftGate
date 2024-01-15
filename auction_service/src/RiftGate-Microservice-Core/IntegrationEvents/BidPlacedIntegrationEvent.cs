using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using IntegrationEvents.Data;
using ProtoBuf;

namespace IntegrationEvents;

[ProtoContract]
public record BidPlacedIntegrationEvent : ISubscribeEvent
{
    [ProtoMember(1)]
    public string AuctionId { get; init; }

    [ProtoMember(2)]
    public Bid Bid { get; init; }
}