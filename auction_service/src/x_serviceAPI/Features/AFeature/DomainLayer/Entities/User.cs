using CodingPatterns.DomainLayer;
using x_serviceAPI.Features.AFeature.DomainLayer.DomainEvents;

namespace x_serviceAPI.Features.AFeature.DomainLayer.Entities;

public class User : Entity, IAggregateRoot
{
    // User properties and methods
    public override int Id { get; }
    public string Username { get; }
    public string Email { get; }

    public void Register()
    {
        // User registration logic...

        // Raise the UserRegisteredEvent
        var userRegisteredEvent = new UserRegisteredDomainEvent(this.Id, this.Username, this.Email);
        this.AddDomainEvent(userRegisteredEvent);
    }


}
