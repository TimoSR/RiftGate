using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using ProtoBuf;

namespace API.Features.UserManagerFeature.ApplicationLayer.Events.Topics.UserManagement;

[ProtoContract]
[TopicName("UserRegInitTopic")]
public class UserRegInitEvent :IPublishIntegrationEvent
{
    public string Message => "User registration Initiated";

    [ProtoMember(1)]
    public string Id { get; set; }
    
    [ProtoMember(2)]
    public string Email { get; set; }
    
    [ProtoMember(3)]
    public string Password { get; set; }
    
}