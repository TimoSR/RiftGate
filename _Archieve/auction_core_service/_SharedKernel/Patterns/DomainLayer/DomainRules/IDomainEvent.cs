using MediatR;

namespace _SharedKernel.Patterns.DomainRules;

public interface IDomainEvent : INotification
{
    string Message { get; }
}