using _SharedKernel.Patterns.InfrastructureLayer.IntegrationEvents;
using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using ProtoBuf;

namespace Application.AppServices.xFeature.V1.IntegrationEvents.Subscribed.EmailValidation;

[ProtoContract]
[TopicName("EmailValidationSuccessTopic")]
public class EmailValidationSuccessEvent : ISubscribeIntegrationEvent
{
    public string Message { get; }
    
    [ProtoMember(1)]
    public string Email { get; set; }
}