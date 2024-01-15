using System.Text.Json;
using Infrastructure.Persistence.MongoDB;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IWebHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Enhanced error details to include request path and method
        var errorDetails = new
        {
            //TraceIdentifier = context.TraceIdentifier,
            //RequestPath = context.Request.Path,
            //RequestMethod = context.Request.Method,
            ExceptionType = exception.GetType().Name,
            //Message = exception.Message
            //StackTrace = exception.StackTrace
        };

        var (statusCode, userMessage) = exception switch
        {
            ArgumentNullException _ => 
                (StatusCodes.Status400BadRequest, exception.Message),

            ArgumentException _ => 
                (StatusCodes.Status400BadRequest, exception.Message),
            
            MongoRepositoryNotFoundException _ => 
                (StatusCodes.Status404NotFound, exception.Message),

            MongoRepositoryConnectionException _ => 
                (StatusCodes.Status503ServiceUnavailable, exception.Message),

            MongoRepositoryException _ => 
                (StatusCodes.Status500InternalServerError, exception.Message),

            _ => 
                (StatusCodes.Status500InternalServerError, exception.Message)
        };

        _logger.LogError(exception, 
            "LOG: Error occurred. TraceIdentifier: {TraceIdentifier}, RequestMethod: {RequestMethod}, RequestPath: {RequestPath}, " +
            "StatusCode: {StatusCode}, Exception: {ExceptionType}", 
            context.TraceIdentifier, context.Request.Method, context.Request.Path, statusCode, exception.GetType().Name);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        //var result = JsonSerializer.Serialize(new { error = userMessage, detail = errorDetails });
        var result = JsonSerializer.Serialize(new { error = userMessage });
        
        await context.Response.WriteAsync(result);
    }
}

