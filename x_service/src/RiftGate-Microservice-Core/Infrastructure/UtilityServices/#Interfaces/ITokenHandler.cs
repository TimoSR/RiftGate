using System.Security.Claims;
using CodingPatterns.InfrastructureLayer.Utilities;

namespace Infrastructure.UtilityServices._Interfaces;

public interface ITokenHandler : IUtilityService
{
    string GenerateJwtToken(string userId);
    ClaimsPrincipal? DecodeJwtToken(string token);
    string GenerateRefreshToken();
}