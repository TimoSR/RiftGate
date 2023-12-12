using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;
using ProtoBuf;

namespace Domain._Shared.Events.Topics.UserManagement;

[ProtoContract]
[TopicName("UserDeletionCompletedTopic")]
public class UserDeletionCompleted : IDomainEvent
{
    public string Email { get; set; }
    public string Message => $"User {Email} Deletion Completed!";
}