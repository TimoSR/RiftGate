using _SharedKernel.Patterns.RegistrationHooks.Utilities;

namespace Infrastructure.Utilities._Interfaces;

public interface IPasswordHasher : IUtilityTool
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}