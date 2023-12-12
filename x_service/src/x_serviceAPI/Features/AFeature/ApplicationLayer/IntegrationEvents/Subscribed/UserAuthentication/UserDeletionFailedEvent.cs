using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using CodingPatterns.Infrastructure.IntegrationEvents;
using ProtoBuf;

namespace x_serviceAPI.Features.AFeature.ApplicationLayer.IntegrationEvents.Subscribed.UserAuthentication;

[ProtoContract]
[TopicName("UserDeletionFailedTopic")]
public class UserDeletionFailedEvent : ISubscribeIntegrationEvent
{
    public string Message => "User auth data deletion failed";
    
    [ProtoMember(1)]
    public string Email { get; set; }

    [ProtoMember(2)]
    public string Reason { get; set; }
}