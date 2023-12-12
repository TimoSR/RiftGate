using MediatR;

namespace DomainLayerLib;

public interface IDomainEvent : INotification
{
    string Message { get; }
}