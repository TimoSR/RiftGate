using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;
using ProtoBuf;

namespace Domain._Shared.Events.Subscribed.UserAuthentication;

[ProtoContract]
[TopicName("UserAuthDetailsSetSuccessTopic")]
public class UserAuthDetailsSetSuccessEvent
{
    public string Message => "User Auth Details Set Succeeded";
    
    [ProtoMember(1)]
    public string Email { get; set; }
}