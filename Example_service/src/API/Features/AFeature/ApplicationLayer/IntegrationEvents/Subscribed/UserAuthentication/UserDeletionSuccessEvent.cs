using _SharedKernel.Patterns.InfrastructureLayer.IntegrationEvents;
using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using ProtoBuf;

namespace Application.AppServices.xFeature.V1.IntegrationEvents.Subscribed.UserAuthentication;

[ProtoContract]
[TopicName("UserDeletionSuccessTopic")]
public class UserDeletionSuccessEvent : ISubscribeIntegrationEvent
{
    public string Message => "User Auth Details Deleted Successfully";

    [ProtoMember(1)]
    public string Email { get; set; }
}