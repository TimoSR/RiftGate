using _SharedKernel.Patterns.DomainRules;
using Domain.DomainEvents;
using Domain.Enums;

namespace Domain.Entities;

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
        var userRegisteredEvent = new UserRegisteredEvent(this.Id, this.Username, this.Email);
        this.AddDomainEvent(userRegisteredEvent);
    }


}
