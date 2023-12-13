using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using ProtoBuf;

namespace API.Features.AFeature.ApplicationLayer.IntegrationEvents.Subscribed.UserAuthentication;

[ProtoContract]
[TopicName("UserDeletionSuccessTopic")]
public class UserDeletionSuccessEvent : ISubscribeIntegrationEvent
{
    public string Message => "User Auth Details Deleted Successfully";

    [ProtoMember(1)]
    public string Email { get; set; }
}