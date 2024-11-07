using CSharpCourse.DesignPatterns.WebApi.Exceptions;
using CSharpCourse.DesignPatterns.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddAuthentication("ApiKey")
    .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthHandler>("ApiKey", opts => { });
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<LoggingMiddleware>();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// We use minimal APIs instead of controllers
app.MapGet("/weatherforecast", [Authorize] async (string? city, IMemoryCache cache, ILogger<Program> logger) =>
{
    if (string.IsNullOrWhiteSpace(city))
    {
        throw new InvalidEndpointParametersException("City parameter is required");
    }

    var cacheKey = $"weather_{city}";

    if (cache.TryGetValue(cacheKey, out WeatherForecast[]? cachedForecast))
    {
        logger.LogInformation("Returning cached forecast for {City}", city);
        return cachedForecast;
    }

    logger.LogInformation("Generating new forecast for {City}", city);

    // Simulate a long-running operation
    await Task.Delay(1000);

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    var cacheOptions = new MemoryCacheEntryOptions()
        .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

    cache.Set(cacheKey, forecast, cacheOptions);

    return forecast;
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
