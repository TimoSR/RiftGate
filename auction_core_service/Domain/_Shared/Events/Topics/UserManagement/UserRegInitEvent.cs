using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;
using ProtoBuf;

namespace Domain._Shared.Events.Topics.UserManagement;

[ProtoContract]
[TopicName("UserRegInitTopic")]
public class UserRegInitEvent : IDomainEvent
{
    public string Message => "User registration Initiated";

    [ProtoMember(1)]
    public string Id { get; set; }
    
    [ProtoMember(2)]
    public string Email { get; set; }
    
    [ProtoMember(3)]
    public string Password { get; set; }
    
}