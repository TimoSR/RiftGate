using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;
using ProtoBuf;

namespace Domain._Shared.Events.Topics.UserManagement;

[ProtoContract]
[TopicName("UserRegCompletedTopic")]
public class UserRegCompletedEvent : IPubEvent
{
    public string Message => "User Registration Completed!";
    
    [ProtoMember(1)]
    public string Email { get; set; }  
}