using CodingPatterns.Infrastructure.Utilities;

namespace Infrastructure.UtilityServices._Interfaces;

public interface IPasswordHasher : IUtilityService
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}