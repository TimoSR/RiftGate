using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using CodingPatterns.Infrastructure.IntegrationEvents;
using ProtoBuf;

namespace x_serviceAPI.Features.AFeature.ApplicationLayer.IntegrationEvents.Subscribed.UserAuthentication;

[ProtoContract]
[TopicName("UserAuthDetailsSetFailedTopic")]
public class UserAuthDetailsSetFailedEvent : ISubscribeIntegrationEvent
{
    public string Message => "User Auth Details Set Failed";
    
    [ProtoMember(1)]
    public string Email { get; set; }
    
    [ProtoMember(2)]
    public string Reason { get; set; }
}
