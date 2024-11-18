using CSharpCourse.DesignPatterns.Behavioral.Observer;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.ObserverTests;

public class GpsServiceTests
{
    [Fact]
    public async Task GpsService()
    {
        var gpsService = new GpsService();

        // Producer
        _ = Task.Run(async () =>
        {
            try
            {
                foreach (var _ in Enumerable.Range(0, 20))
                {
                    await Task.Delay(50);

                    await gpsService.PublishAsync(
                        new Coordinates(
                            Guid.NewGuid(),
                            Random.Shared.NextDouble() * 180 - 90,
                            Random.Shared.NextDouble() * 360 - 180));
                }
            }
            finally
            {
                // Complete the writer when we are done, otherwise
                // the consumer will be listening forever.
                gpsService.TryCompleteWriter();
            }
        });

        var count = 0;

        // Consumer (will process the coordinates until the writer is completed)
        await foreach (var coordinates in gpsService.ReadCoordinatesAsync())
        {
            count++;
        }

        Assert.Equal(20, count);
    }
}
