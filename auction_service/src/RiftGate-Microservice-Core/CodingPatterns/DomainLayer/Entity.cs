using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CodingPatterns.DomainLayer;

public abstract class Entity : IEntity
{
    [BsonId] [BsonRepresentation(BsonType.ObjectId)] public string Id { get; set; }
    
    private readonly List<INotification>? _domainEvents = new ();
    
    public IReadOnlyCollection<INotification>? DomainEvents => _domainEvents?.AsReadOnly();

    public bool IsDeleted { get; private set; }
    
    public virtual void MarkAsDeleted<T>() where T : IEntity
    {
        IsDeleted = true;
        AddDomainEvent(new EntitySoftDeletedEvent<T>(Id));
    }
    
    public void AddDomainEvent(INotification eventItem)
    {
        _domainEvents?.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        _domainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
    
    //... Additional code such as equality checks and other entity-related operations
}