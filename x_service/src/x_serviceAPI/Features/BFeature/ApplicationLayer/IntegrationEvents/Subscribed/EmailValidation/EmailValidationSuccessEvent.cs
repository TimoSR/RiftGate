namespace x_serviceAPI.Features.BFeature.ApplicationLayer.IntegrationEvents.Subscribed.EmailValidation;

[ProtoContract]
[TopicName("EmailValidationSuccessTopic")]
public class EmailValidationSuccessEvent : ISubscribeIntegrationEvent
{
    public string Message { get; }
    
    [ProtoMember(1)]
    public string Email { get; set; }
}