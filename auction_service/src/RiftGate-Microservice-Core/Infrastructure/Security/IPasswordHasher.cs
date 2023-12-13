using CodingPatterns.InfrastructureLayer.Utilities;

namespace Infrastructure.UtilityServices._Interfaces;

public interface IPasswordHasher : IFrastructureService
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}