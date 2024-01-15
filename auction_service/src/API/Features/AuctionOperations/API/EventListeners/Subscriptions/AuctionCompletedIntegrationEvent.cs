using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using CodingPatterns.InfrastructureLayer.IntegrationEvents.GooglePubSub._Attributes;
using ProtoBuf;

namespace API.Features.AuctionOperations.API.EventListeners.Subscriptions;

[ProtoContract]
[TopicName("AuctionCompletedTopic")]
public record AuctionCompletedIntegrationEvent : ISubscribeEvent
{
    [ProtoMember(1)]
    public string AuctionId { get; init; }

    [ProtoMember(2)]
    public DateTime CompletionTime { get; init; }
}