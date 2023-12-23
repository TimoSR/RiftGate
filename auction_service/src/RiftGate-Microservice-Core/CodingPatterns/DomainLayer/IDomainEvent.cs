using MediatR;

namespace CodingPatterns.DomainLayer;

public interface IDomainEvent : INotification
{
    public string Message { get; }
}