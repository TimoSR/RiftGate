using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using ProtoBuf;

namespace API.Features.UserManagerFeature.ApplicationLayer.Events.Topics.UserManagement;

[ProtoContract]
[TopicName("UserDeletionInitTopic")]
public class UserDeletionInitEvent : IPublishIntegrationEvent
{
    public string Message => "User Deletion Initiated";
    
    [ProtoMember(1)]
    public string Email { get; set; }
}