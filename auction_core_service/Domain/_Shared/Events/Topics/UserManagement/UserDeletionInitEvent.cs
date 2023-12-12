using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;
using ProtoBuf;

namespace Domain._Shared.Events.Topics.UserManagement;

[ProtoContract]
[TopicName("UserDeletionInitTopic")]
public class UserDeletionInitEvent : IDomainEvent
{
    public string Message => "User Deletion Initiated";
    
    [ProtoMember(1)]
    public string Email { get; set; }
}