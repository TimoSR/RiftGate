using MediatR;
using MongoDB.Bson.Serialization.Attributes;

namespace CodingPatterns.DomainLayer;

public interface IAggregateRoot : IEntity
{
    [BsonIgnore]
    List<INotification> DomainEvents { get; }
    void AddDomainEvent(INotification eventItem);
    void RemoveDomainEvent(INotification eventItem);
    void ClearDomainEvents();
}