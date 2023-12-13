using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;
using ProtoBuf;

namespace Domain._Shared.Events.Subscribed.UserAuthentication;

[ProtoContract]
[TopicName("UserDeletionSuccessTopic")]
public class UserDeletionSuccessEvent
{
    public string Message => "User Auth Details Deleted Successfully";

    [ProtoMember(1)]
    public string Email { get; set; }
}