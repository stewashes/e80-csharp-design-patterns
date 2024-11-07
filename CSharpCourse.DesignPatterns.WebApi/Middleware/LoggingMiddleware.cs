using System.Diagnostics;

namespace CSharpCourse.DesignPatterns.WebApi.Middleware;

// This is not always needed in production, since we can use
// Asp.Net Core's built-in logging middleware and simply adjust
// the log level to log only the necessary information.
public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation("Processing request: {Method} {Path}",
            context.Request.Method,
            context.Request.Path);

        var startTime = Stopwatch.GetTimestamp();

        try
        {
            await _next(context);
        }
        finally
        {
            _logger.LogInformation(
                "Completed request with status code: {StatusCode} in {ElapsedMilliseconds}ms",
                context.Response.StatusCode,
                Stopwatch.GetElapsedTime(startTime));
        }
    }
}
