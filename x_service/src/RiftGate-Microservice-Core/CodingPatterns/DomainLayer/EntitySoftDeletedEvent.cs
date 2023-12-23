namespace CodingPatterns.DomainLayer;

public class EntitySoftDeletedEvent<T>(string id) : IDomainEvent where T : IEntity
{
    public string Message => $"{nameof(T)} with id {id} was successfully deleted.";
}