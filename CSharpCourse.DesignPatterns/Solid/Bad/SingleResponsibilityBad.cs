using System.Text.Json;
using System.Text.Json.Serialization;
using CSharpCourse.DesignPatterns.Solid.Enums;

namespace CSharpCourse.DesignPatterns.Solid.Bad;

internal class WeatherData
{
    public DateTime DateTime { get; init; }
    public double Temperature { get; init; }
    public double Humidity { get; init; }
    public WeatherCondition Condition { get; init; }
    public string? Location { get; init; }
    public double RainProbability { get; init; }

    public async Task<bool> IsValidLocationAsync()
    {
        if (string.IsNullOrWhiteSpace(Location))
        {
            return false;
        }

        // Imagine we call an external API here
        await Task.Delay(100);
        
        return true;
    }

    public async Task SaveAsync(string fileName)
    {
        await File.WriteAllTextAsync(fileName, JsonSerializer.Serialize(this));
    }

    public static async Task<WeatherData?> LoadAsync(string fileName)
    {
        var json = await File.ReadAllTextAsync(fileName);
        return JsonSerializer.Deserialize<WeatherData>(json);
    }
}
