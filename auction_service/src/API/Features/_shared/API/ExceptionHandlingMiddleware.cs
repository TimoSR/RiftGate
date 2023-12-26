using System.Text.Json;
using Infrastructure.Persistence.MongoDB;

namespace API.Features._shared.API;

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

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        // Enhanced error details to include request path and method
        var errorDetails = new
        {
            //TraceIdentifier = context.TraceIdentifier,
            //RequestPath = context.Request.Path,
            //RequestMethod = context.Request.Method,
            ExceptionType = exception.GetType().Name,
            Message = exception.Message
            //StackTrace = exception.StackTrace
        };

        var (statusCode, userMessage) = exception switch
        {
            MongoRepositoryNotFoundException _ => 
                (StatusCodes.Status404NotFound, "The specified resource was not found."),
        
            ArgumentNullException _ => 
                (StatusCodes.Status400BadRequest, "Required information was missing."),
        
            ArgumentException _ => 
                (StatusCodes.Status400BadRequest, "The argument provided was not valid."),
        
            MongoRepositoryConnectionException _ => 
                (StatusCodes.Status503ServiceUnavailable, "There was an issue connecting to the database."),
        
            MongoRepositoryException _ => 
                (StatusCodes.Status500InternalServerError, "There was an issue with the database operation."),
        
            _ => 
                (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        _logger.LogError(exception, 
            "LOG: Error occurred. TraceIdentifier: {TraceIdentifier}, RequestMethod: {RequestMethod}, RequestPath: {RequestPath}, " +
            "StatusCode: {StatusCode}, Exception: {ExceptionType}", 
            context.TraceIdentifier, context.Request.Method, context.Request.Path, statusCode, exception.GetType().Name);

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var result = JsonSerializer.Serialize(new { error = userMessage, detail = errorDetails });

        return context.Response.WriteAsync(result);
    }
}
