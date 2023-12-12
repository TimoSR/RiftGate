using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Middleware;

public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var watch = Stopwatch.StartNew();
        _logger.LogInformation("Incoming request: {Method} {Path}", context.Request.Method, context.Request.Path);

        await _next(context);

        watch.Stop();
        var statusCode = context.Response.StatusCode;
        var statusCodeName = ((HttpStatusCode)statusCode).ToString();

        _logger.LogInformation("Request completed with status code: {StatusCode}:{StatusCodeName}, in {ElapsedMilliseconds}ms", 
            statusCode, statusCodeName, watch.ElapsedMilliseconds);
    }
}