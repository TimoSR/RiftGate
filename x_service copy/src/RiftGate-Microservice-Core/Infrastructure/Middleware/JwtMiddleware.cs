using System.Text.Json;
using Infrastructure.UtilityServices._Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Middleware;

public class ErrorResponse
{
    public string Message { get; set; }

    public ErrorResponse(string message)
    {
        Message = message;
    }

    public string ToJson() => JsonSerializer.Serialize(this);
}

public class JwtMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ITokenHandler _tokenHandler;
    private readonly ILogger<JwtMiddleware> _logger;

    public JwtMiddleware(RequestDelegate next, ITokenHandler tokenHandler, ILogger<JwtMiddleware> logger)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _tokenHandler = tokenHandler ?? throw new ArgumentNullException(nameof(tokenHandler));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
        {
            await _next(context);
            return;
        }
        
        // Add the route you want to allow without JWT here
        if (context.Request.Path.Value.StartsWith("/graphql", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }
        
        if (!context.Request.Headers.TryGetValue(AuthConstants.AuthorizationHeader, out var authHeader))
        {
            await RespondWithUnauthorized(context, "Missing Authorization header.");
            return;
        }

        var header = authHeader.ToString();
        if (!header.StartsWith(AuthConstants.BearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            await RespondWithBadRequest(context, "Authorization header must start with 'Bearer'.");
            return;
        }

        var token = header.Substring(AuthConstants.BearerPrefix.Length).Trim();
        if (string.IsNullOrEmpty(token))
        {
            await RespondWithBadRequest(context, "Bearer token is empty.");
            return;
        }

        try
        {
            var principal = _tokenHandler.DecodeJwtToken(token);
            context.User = principal;
            await _next(context);
        }
        catch (SecurityTokenExpiredException)
        {
            await RespondWithUnauthorized(context, "Token is expired.");
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            await RespondWithUnauthorized(context, "Invalid token signature.");
        }
        catch (SecurityTokenException)
        {
            await RespondWithUnauthorized(context, "Token validation failed.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during token validation.");
            await RespondWithInternalServerError(context, "An unexpected error occurred.");
        }
    }

    private async Task RespondWithError(HttpContext context, int statusCode, string message)
    {
        var errorResponse = new ErrorResponse(message);
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(errorResponse.ToJson());
    }

    private Task RespondWithBadRequest(HttpContext context, string message)
    {
        return RespondWithError(context, StatusCodes.Status400BadRequest, message);
    }

    private Task RespondWithUnauthorized(HttpContext context, string message)
    {
        return RespondWithError(context, StatusCodes.Status401Unauthorized, message);
    }

    private Task RespondWithInternalServerError(HttpContext context, string message)
    {
        return RespondWithError(context, StatusCodes.Status500InternalServerError, message);
    }
}
