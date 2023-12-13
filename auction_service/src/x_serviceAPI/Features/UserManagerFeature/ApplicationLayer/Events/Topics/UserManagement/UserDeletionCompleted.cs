using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using ProtoBuf;

namespace x_serviceAPI.Features.UserManagerFeature.ApplicationLayer.Events.Topics.UserManagement;

[ProtoContract]
[TopicName("UserDeletionCompletedTopic")]
public class UserDeletionCompleted : IPublishIntegrationEvent
{
    public string Email { get; set; }
    public string Message => $"User {Email} Deletion Completed!";
}