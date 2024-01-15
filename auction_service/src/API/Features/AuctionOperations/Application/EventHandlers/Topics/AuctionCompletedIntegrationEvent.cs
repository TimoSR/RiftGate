using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using CodingPatterns.InfrastructureLayer.IntegrationEvents.GooglePubSub._Attributes;
using ProtoBuf;
using System;

namespace API.Features.AuctionOperations.Application.EventHandlers.Topics;

[ProtoContract]
[TopicName("AuctionCompletedTopic")]
public record AuctionCompletedIntegrationEvent : IPublishEvent
{
    [ProtoMember(1)]
    public string AuctionId { get; init; }

    [ProtoMember(2)]
    public DateTime CompletionTime { get; init; }

    public AuctionCompletedIntegrationEvent(string auctionId, DateTime completionTime)
    {
        AuctionId = auctionId;
        CompletionTime = completionTime;
    }

    public string Message => 
        $"Auction {AuctionId} completed at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}