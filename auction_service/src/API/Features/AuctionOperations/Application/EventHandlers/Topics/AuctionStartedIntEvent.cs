using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using CodingPatterns.InfrastructureLayer.IntegrationEvents.GooglePubSub._Attributes;
using ProtoBuf;
using System;

namespace API.Features.AuctionOperations.Application.EventHandlers.Topics;

[ProtoContract]
[TopicName("AuctionStartedTopic")]
public readonly record struct AuctionStartedIntEvent : IPublishEvent
{
    [ProtoMember(1)]
    public string AuctionId { get; init; }

    [ProtoMember(2)]
    public DateTime StartTime { get; init; }

    public AuctionStartedIntEvent(string auctionId, DateTime startTime)
    {
        AuctionId = auctionId;
        StartTime = startTime;
    }

    public string Message => 
        $"Event: Auction {AuctionId} started at {StartTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}