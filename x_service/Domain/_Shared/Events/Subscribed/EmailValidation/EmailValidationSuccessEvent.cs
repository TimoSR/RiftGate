using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;
using ProtoBuf;

namespace Domain._Shared.Events.Subscribed.EmailValidation;

[ProtoContract]
[TopicName("EmailValidationSuccessTopic")]
public class EmailValidationSuccessEvent
{
    public string Message { get; }
    
    [ProtoMember(1)]
    public string Email { get; set; }
}