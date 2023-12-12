using Domain.UserManagement.Entities;

namespace Domain.UserManagement.Messages;

public static class UserRegMsg
{
    public const string InvalidEmail = "Invalid email address";
    public static readonly string InvalidPassword = $"Password must have a minimum length of {User.MinPasswordLength} and include at least one uppercase letter, number, and special symbol (e.g., !@#$%^&*).";
    public const string Successful = "User successfully registered";
    public const string EmailAlreadyExists = "Email already registered";
}