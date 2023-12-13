using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Middleware;

public class DetailedLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DetailedLoggingMiddleware> _logger;

    public DetailedLoggingMiddleware(RequestDelegate next, ILogger<DetailedLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var watch = Stopwatch.StartNew();

        // Using a scope to correlate logs within the same request
        using (_logger.BeginScope($"RequestId: {Guid.NewGuid()}"))
        {
            try
            {
                _logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path}");
                _logger.LogInformation($"Client IP: {context.Connection.RemoteIpAddress}");
                LogRequestHeaders(context.Request);

                await _next(context);

                watch.Stop();
                _logger.LogInformation($"Response completed with status code: {context.Response.StatusCode}, in {watch.ElapsedMilliseconds}ms");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during the request.");
                throw; // Rethrow the exception after logging it
            }
        }
    }

    private void LogRequestHeaders(HttpRequest request)
    {
        foreach (var header in request.Headers)
        {
            _logger.LogInformation($"Header: {header.Key} Value: {header.Value}");
        }
    }
}

// Remember to register this middleware in your Startup.cs or Program.cs