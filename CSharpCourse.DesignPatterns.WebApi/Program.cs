using CSharpCourse.DesignPatterns.WebApi.Exceptions;
using CSharpCourse.DesignPatterns.WebApi.Middleware;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Memory;

var builder = WebApplication.CreateBuilder(args);

// TODO: Add services to the container.

var app = builder.Build();

// TODO: Configure the HTTP request pipeline.

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// We use minimal APIs instead of controllers
// TODO: Add authorization
// TODO: Add caching
// TODO: Add logging
app.MapGet("/weatherforecast", async (string? city) =>
{
    if (string.IsNullOrWhiteSpace(city))
    {
        throw new InvalidEndpointParametersException("City parameter is required");
    }

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

    return forecast;
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
