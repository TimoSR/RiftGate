using Infrastructure.UtilityServices._Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.UtilityServices.Hashing;

public class PasswordPasswordHasher : IPasswordHasher
{
    private readonly IPasswordHasher<object> _passwordHasher = new PasswordHasher<object>();

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(null, password);
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(null, hashedPassword, providedPassword) != PasswordVerificationResult.Failed;
    }
}