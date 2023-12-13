using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using ProtoBuf;

namespace API.Features.UserManagerFeature.ApplicationLayer.Events.Subscribed.EmailValidation;

[ProtoContract]
[TopicName("EmailValidationSuccessTopic")]
public class EmailValidationSuccessEvent : ISubscribeIntegrationEvent
{
    public string Message { get; }
    
    [ProtoMember(1)]
    public string Email { get; set; }
}