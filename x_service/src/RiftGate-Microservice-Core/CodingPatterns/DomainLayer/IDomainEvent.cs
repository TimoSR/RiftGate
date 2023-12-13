using MediatR;

namespace CodingPatterns.DomainLayer;

public interface IDomainEvent : INotification
{
    string Message { get; }
}