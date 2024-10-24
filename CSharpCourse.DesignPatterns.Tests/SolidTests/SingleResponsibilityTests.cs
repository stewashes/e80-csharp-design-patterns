using CSharpCourse.DesignPatterns.Solid.Enums;

namespace CSharpCourse.DesignPatterns.Tests.SolidTests;

public class SingleResponsibilityTests
{
    [Fact]
    public async Task Bad()
    {
        var weatherData = new Solid.Bad.WeatherData
        {
            DateTime = DateTime.Now,
            Temperature = 15.5,
            Humidity = 0.3,
            Condition = WeatherCondition.Sunny,
            Location = "Milan",
            RainProbability = 0.05
        };
        
        Assert.True(await weatherData.IsValidLocationAsync());

        var tempFile = Path.GetRandomFileName();
        await weatherData.SaveAsync(tempFile);
        
        var loadedWeatherData = await Solid.Bad.WeatherData.LoadAsync(tempFile);
        Assert.NotNull(loadedWeatherData);
        Assert.Equal(weatherData.Location, loadedWeatherData.Location);
    }

    [Fact]
    public async Task Good()
    {
        var weatherData = new Solid.Good.WeatherData
        {
            DateTime = DateTime.Now,
            Temperature = 15.5,
            Humidity = 0.3,
            Condition = WeatherCondition.Sunny,
            Location = "Milan",
            RainProbability = 0.05
        };

        var locationService = new Solid.Good.FakeLocationService();
        
        Assert.True(await locationService.IsValidLocationAsync(weatherData.Location));

        var tempFile = Path.GetRandomFileName();
        
        var storageService = new Solid.Good.LocalWeatherDataStorageService();
        await storageService.SaveAsync(weatherData, tempFile);
        
        var loadedWeatherData = await storageService.LoadAsync(tempFile);
        Assert.NotNull(loadedWeatherData);
        Assert.Equal(weatherData.Location, loadedWeatherData.Location);
    }
    
    [Fact]
    public async Task Better()
    {
        var weatherData = new Solid.Better.WeatherData
        {
            DateTime = DateTime.Now,
            Temperature = 15.5,
            Humidity = 0.3,
            Condition = WeatherCondition.Sunny,
            Location = "Milan",
            RainProbability = 0.05
        };

        var locationService = new Solid.Better.FakeLocationService();
        
        Assert.True(await locationService.IsValidLocationAsync(weatherData.Location));

        var tempFile = Path.GetRandomFileName();
        
        var storageService = new Solid.Better.LocalStorageService<Solid.Better.WeatherData>();
        await storageService.SaveAsync(weatherData, tempFile);
        
        var loadedWeatherData = await storageService.LoadAsync(tempFile);
        Assert.NotNull(loadedWeatherData);
        Assert.Equal(weatherData.Location, loadedWeatherData.Location);
    }
}
