using CodingPatterns.DomainLayer;

namespace x_serviceAPI.Features.AFeature.DomainLayer.DomainEvents;

public class UserRegisteredDomainEvent : IDomainEvent
{
    public string Message { get; }
    public int UserId { get; }
    public string Username { get; }
    public string Email { get; }
    // Add other relevant properties

    public UserRegisteredDomainEvent(int userId, string username, string email)
    {
        UserId = userId;
        Username = username;
        Email = email;
        // Initialize other properties
    }

 
}
