namespace x_serviceAPI.Features.AFeature.ApplicationLayer.IntegrationEvents.Subscribed.UserAuthentication;

[ProtoContract]
[TopicName("UserAuthDetailsSetSuccessTopic")]
public class UserAuthDetailsSetSuccessEvent : ISubscribeIntegrationEvent
{
    public string Message => "User Auth Details Set Succeeded";
    
    [ProtoMember(1)]
    public string Email { get; set; }
}