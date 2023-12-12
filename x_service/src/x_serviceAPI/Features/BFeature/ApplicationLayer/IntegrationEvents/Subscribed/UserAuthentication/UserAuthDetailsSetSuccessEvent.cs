using _SharedKernel.Patterns.InfrastructureLayer.IntegrationEvents;
using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using ProtoBuf;

namespace Application.AppServices.xFeature.V1.IntegrationEvents.Subscribed.UserAuthentication;

[ProtoContract]
[TopicName("UserAuthDetailsSetSuccessTopic")]
public class UserAuthDetailsSetSuccessEvent : ISubscribeIntegrationEvent
{
    public string Message => "User Auth Details Set Succeeded";
    
    [ProtoMember(1)]
    public string Email { get; set; }
}