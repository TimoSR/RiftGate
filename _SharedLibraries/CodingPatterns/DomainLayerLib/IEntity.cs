using MediatR;

namespace DomainLayerLib;

public interface IEntity
{
    int Id { get; }
    IReadOnlyCollection<INotification> DomainEvents { get; }
    void AddDomainEvent(INotification eventItem);
    void RemoveDomainEvent(INotification eventItem);
    void ClearDomainEvents();
}