using System.Security.Claims;
using _SharedKernel.Patterns.RegistrationHooks.Utilities;

namespace Infrastructure.Utilities._Interfaces;

public interface ITokenHandler : IUtilityTool
{
    string GenerateJwtToken(string userId);
    ClaimsPrincipal? DecodeJwtToken(string token);
    string GenerateRefreshToken();
}