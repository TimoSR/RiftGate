using Domain.Enums;

namespace Domain.Entities;

public class User
{
    public int UserId { get; set; } // Unique identifier matching the User Management service
    public string DisplayName { get; set; } // User's display name for auction purposes
    public UserRole Role { get; set; }
    public virtual ICollection<Bid> Bids { get; set; } // Collection of Bids
}