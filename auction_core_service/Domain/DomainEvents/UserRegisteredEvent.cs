using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;

namespace Domain.DomainEvents;

public class UserRegisteredEvent : IDomainEvent
{
    public string Message { get; }
    public int UserId { get; }
    public string Username { get; }
    public string Email { get; }
    // Add other relevant properties

    public UserRegisteredEvent(int userId, string username, string email)
    {
        UserId = userId;
        Username = username;
        Email = email;
        // Initialize other properties
    }

 
}
