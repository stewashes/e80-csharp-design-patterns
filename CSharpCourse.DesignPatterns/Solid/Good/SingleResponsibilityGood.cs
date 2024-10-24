using System.Text.Json;
using CSharpCourse.DesignPatterns.Solid.Enums;

namespace CSharpCourse.DesignPatterns.Solid.Good;

// The only purpose of this record is to hold weather data
internal record WeatherData
{
    public DateTime DateTime { get; init; }
    public double Temperature { get; init; }
    public double Humidity { get; init; }
    public WeatherCondition Condition { get; init; }
    public string? Location { get; init; }
    public double RainProbability { get; init; }
}

// The only purpose of this class is providing information
// about locations
internal class FakeLocationService
{
    public async Task<bool> IsValidLocationAsync(string location)
    {
        await Task.Delay(100);
        return true;
    }
}

// The only purpose of this class is to save and load weather data
// to and from local files
internal class LocalWeatherDataStorageService
{
    public async Task SaveAsync(WeatherData weatherData, string fileName)
    {
        await File.WriteAllTextAsync(fileName, JsonSerializer.Serialize(weatherData));
    }

    public async Task<WeatherData?> LoadAsync(string fileName)
    {
        var json = await File.ReadAllTextAsync(fileName);
        return JsonSerializer.Deserialize<WeatherData>(json);
    }
}
