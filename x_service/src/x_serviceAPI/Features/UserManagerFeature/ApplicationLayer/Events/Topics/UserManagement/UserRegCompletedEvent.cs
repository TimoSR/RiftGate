using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using ProtoBuf;

namespace x_serviceAPI.Features.UserManagerFeature.ApplicationLayer.Events.Topics.UserManagement;

[ProtoContract]
[TopicName("UserRegCompletedTopic")]
public class UserRegCompletedEvent : IPublishIntegrationEvent
{
    public string Message => "User Registration Completed!";
    
    [ProtoMember(1)]
    public string Email { get; set; }  
}