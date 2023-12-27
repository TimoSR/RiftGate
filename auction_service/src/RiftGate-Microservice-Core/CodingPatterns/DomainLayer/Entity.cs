using MediatR;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace CodingPatterns.DomainLayer;

public abstract class Entity : IEntity
{
    static Entity()
    {
        BsonClassMap.RegisterClassMap<Entity>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
            cm.MapIdMember(c => c.Id);
            cm.UnmapMember(c => c.DomainEvents); // Ignore the DomainEvents
        });
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; protected init; } = ObjectId.GenerateNewId().ToString();

    [BsonIgnore] public List<INotification>? DomainEvents { get; private set; }

    public bool IsDeleted { get; private set; }

    public virtual void MarkAsDeleted<T>() where T : IEntity
    {
        IsDeleted = true;
        AddDomainEvent(new EntitySoftDeletedEvent<T>(Id));
    }

    public void TriggerDeleteNotification<T>() where T : IEntity
    {
        AddDomainEvent(new EntityDeletedEvent<T>(Id));
    }

    public void AddDomainEvent(INotification eventItem)
    {
        DomainEvents ??= new List<INotification>();
        DomainEvents?.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        DomainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        DomainEvents?.Clear();
    }
}