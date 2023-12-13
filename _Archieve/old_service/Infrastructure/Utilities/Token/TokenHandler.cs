using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Infrastructure.Utilities._Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Utilities.Token;

public class TokenHandler : ITokenHandler
{
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<TokenHandler> _logger;

    public TokenHandler(IConfiguration configuration, ILogger<TokenHandler> logger)
    {
        _jwtSettings = new JwtSettings
        {
            PlaintextKey = configuration.JwtKey,
            EncryptionKey = configuration.JwtEncryptionKey,
            Issuer = configuration.JwtIssuer,
            Audience = configuration.JwtAudience,
            ExpirationInHours = 6
        };
       
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public string GenerateJweToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var plaintextKey = Convert.FromBase64String(_jwtSettings.PlaintextKey);
        var securityKey = new SymmetricSecurityKey(plaintextKey);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", userId) }),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationInHours),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            EncryptingCredentials = new EncryptingCredentials(_jwtSettings.EncryptionKey, SecurityAlgorithms.RsaOAEP, SecurityAlgorithms.Aes256CbcHmacSha512),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
        var jwe = tokenHandler.WriteToken(token);
        return jwe;
    }
    
    public ClaimsPrincipal DecodeJweToken(string jweToken)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                TokenDecryptionKey = _jwtSettings.EncryptionKey,
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero
            };

            return tokenHandler.ValidateToken(jweToken, validationParameters, out _);
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.LogWarning($"Token expired: {ex.Message}");
            throw;
        }
        catch (SecurityTokenInvalidSignatureException ex)
        {
            _logger.LogWarning($"Token has an invalid signature: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while decoding the token: {ex.Message}");
            throw;
        }
    }
    
    public string GenerateJwtToken(string userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var plaintextKey = Convert.FromBase64String(_jwtSettings.PlaintextKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", userId) }),
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.ExpirationInHours),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(plaintextKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public ClaimsPrincipal DecodeJwtToken(string token)
    {
        try
        {
            var key = Convert.FromBase64String(_jwtSettings.PlaintextKey);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = true,
                ValidAudience = _jwtSettings.Audience,
                ClockSkew = TimeSpan.Zero
            };

            return new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.LogWarning($"Token expired: {ex.Message}");
            throw;
        }
        catch (SecurityTokenInvalidSignatureException ex)
        {
            _logger.LogWarning($"Token has an invalid signature: {ex.Message}");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while decoding the token: {ex.Message}");
            throw;
        }
    }

    public string GenerateRefreshToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var randomBytes = new byte[32];
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }
}

public class JwtSettings
{
    public string PlaintextKey { get; set; }
    public RsaSecurityKey? EncryptionKey { get; set; }
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationInHours { get; set; }
}