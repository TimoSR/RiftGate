using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;
using ProtoBuf;

namespace Domain._Shared.Events.Subscribed.UserAuthentication;

[ProtoContract]
[TopicName("UserAuthDetailsSetFailedTopic")]
public class UserAuthDetailsSetFailedEvent
{
    public string Message => "User Auth Details Set Failed";
    
    [ProtoMember(1)]
    public string Email { get; set; }
    
    [ProtoMember(2)]
    public string Reason { get; set; }
}
