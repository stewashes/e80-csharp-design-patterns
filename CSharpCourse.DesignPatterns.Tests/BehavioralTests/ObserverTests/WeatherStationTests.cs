using CSharpCourse.DesignPatterns.Behavioral.Observer;
using CSharpCourse.DesignPatterns.Tests.Utils;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.ObserverTests;

public class WeatherStationTests
{
    [Fact]
    public void WeatherStation()
    {
        var weatherStation = new WeatherStation();
        var temperatureLogger = new TemperatureLogger();
        var temperatureNotifier = new TemperatureNotifier();

        weatherStation.Attach(temperatureLogger);

        var output = string.Empty;

        // We must detach the observer to prevent memory leaks!
        try
        {
            output = OutputUtils.CaptureConsoleOutput(
                () => weatherStation.SetTemperature(25));

            Assert.Contains("Temperature at", output);

            weatherStation.Attach(temperatureNotifier);

            // We must detach the observer to prevent memory leaks!
            try
            {
                output = OutputUtils.CaptureConsoleOutput(
                () => weatherStation.SetTemperature(-5));

                Assert.Contains("Temperature is below zero!", output);
            }
            finally
            {
                weatherStation.Detach(temperatureNotifier);
            }
        }
        finally
        {
            weatherStation.Detach(temperatureLogger);
        }

        output = OutputUtils.CaptureConsoleOutput(
            () => weatherStation.SetTemperature(30));

        Assert.DoesNotContain("Temperature at", output);

        // ... there is a better way to do this, using the IDisposable pattern
    }
}
