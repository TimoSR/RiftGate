using MediatR;

namespace _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;

public interface IDomainEvent : INotification
{
    string Message { get; }
}