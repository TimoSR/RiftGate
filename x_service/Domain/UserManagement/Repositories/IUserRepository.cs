using Domain.UserManagement.Entities;
using Domain.UserManagement.Enums;

namespace Domain.UserManagement.Repositories;

public interface IUserRepository
{   
    // Read Operations
    Task<User> GetUserByIdAsync(string id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByEmailAsync(string email);

    // Create Operation
    Task<bool> CreateUserIfNotRegisteredAsync(User newUser);

    // Update Operations
    Task UpdateUserAsync(User user);
    Task UpdateUserEmailAsync(string userId, string newEmail);
    Task UpdateUserPasswordAsync(string userId, string newPassword);
    Task<bool> UpdateUserStatusByEmailAsync(string email, UserStatus newStatus);

    // Delete Operation
    Task<bool> DeleteUserAsync(string id);
    Task<bool> DeleteUserByEmailAsync(string email);
}