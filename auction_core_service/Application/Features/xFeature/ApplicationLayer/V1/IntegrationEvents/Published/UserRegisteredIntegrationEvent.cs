using _SharedKernel.Patterns.InfrastructureLayer.IntegrationEvents;
using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using ProtoBuf;

namespace Application.AppServices.xFeature.V1.IntegrationEvents.Published;

[ProtoContract]
[TopicName("UserRegisteredTopic")]
public class UserRegisteredIntegrationEvent : IPublishIntegrationEvent
{
    public string Message { get; }
    [ProtoMember(1)] private int UserId { get; }
    [ProtoMember(2)] private string Username { get; }
    [ProtoMember(3)] private string Email { get; }

    public UserRegisteredIntegrationEvent(int userId, string username, string email)
    {
        UserId = userId;
        Username = username;
        Email = email;
        Message = $"User {Username} with ID {UserId} has registered with email {Email}.";
    }
}