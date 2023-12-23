namespace CodingPatterns.DomainLayer;

public class EntityDeletedEvent<T>(string id) : IDomainEvent where T : IEntity
{
    public string Message => $"{nameof(T)} with id {id} was successfully soft-deleted.";
}