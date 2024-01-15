using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using ProtoBuf;

namespace IntegrationEvents;

[ProtoContract]
public record AuctionCompletedIntegrationEvent : ISubscribeEvent
{
    [ProtoMember(1)]
    public string AuctionId { get; init; }

    [ProtoMember(2)]
    public DateTime CompletionTime { get; init; }
}