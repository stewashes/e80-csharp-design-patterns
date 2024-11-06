using CSharpCourse.DesignPatterns.Assignments;
using CSharpCourse.DesignPatterns.Tests.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpCourse.DesignPatterns.Tests.AssignmentTests;

public class PerformanceMonitoringProxyTests
{
    [Fact]
    public void DoSomething()
    {
        // This is fast and should not get logged
        var slowService = new SlowService(TimeSpan.FromMilliseconds(10));

        var proxy = PerformanceMonitoringProxy<IService>.Create(
            slowService, threshold: TimeSpan.FromMilliseconds(200));

        var output = OutputUtils.CaptureConsoleOutput(proxy.DoSomething);

        Assert.DoesNotContain("Method DoSomething took", output);

        // This is slow and should get logged
        slowService = new SlowService(TimeSpan.FromMilliseconds(200));

        proxy = PerformanceMonitoringProxy<IService>.Create(
            slowService, threshold: TimeSpan.FromMilliseconds(10));

        output = OutputUtils.CaptureConsoleOutput(proxy.DoSomething);

        Assert.Contains("Method DoSomething took", output);
    }

    [Fact]
    public async Task DoSomethingAsync()
    {
        // This is fast and should not get logged
        var slowService = new SlowService(TimeSpan.FromMilliseconds(10));

        var proxy = PerformanceMonitoringProxy<IService>.Create(
            slowService, threshold: TimeSpan.FromMilliseconds(200));

        var output = await OutputUtils.CaptureConsoleOutputAsync(
            proxy.DoSomethingAsync);

        Assert.DoesNotContain("Method DoSomethingAsync took", output);

        // This is slow and should get logged
        slowService = new SlowService(TimeSpan.FromMilliseconds(200));

        proxy = PerformanceMonitoringProxy<IService>.Create(
            slowService, threshold: TimeSpan.FromMilliseconds(10));

        output = await OutputUtils.CaptureConsoleOutputAsync(
            proxy.DoSomethingAsync);

        Assert.Contains("Method DoSomethingAsync took", output);
    }

    [Fact]
    public async Task GetResultAsync()
    {
        // This is fast and should not get logged
        var slowService = new SlowService(TimeSpan.FromMilliseconds(10));

        var proxy = PerformanceMonitoringProxy<IService>.Create(
            slowService, threshold: TimeSpan.FromMilliseconds(200));

        var result = false;
        var output = await OutputUtils.CaptureConsoleOutputAsync(
            async () =>
            {
                result = await proxy.GetResultAsync();
            });

        // Make sure the return value is correct
        Assert.True(result);

        Assert.DoesNotContain("Method GetResultAsync took", output);

        // This is slow and should get logged
        slowService = new SlowService(TimeSpan.FromMilliseconds(200));

        proxy = PerformanceMonitoringProxy<IService>.Create(
            slowService, threshold: TimeSpan.FromMilliseconds(10));

        output = await OutputUtils.CaptureConsoleOutputAsync(
            async () =>
            {
                result = await proxy.GetResultAsync();
            });

        Assert.Contains("Method GetResultAsync took", output);
    }

    // NOTE: The ValueTask and ValueTask<T> cases are not covered

    [Fact]
    public void DependencyInjection()
    {
        var services = new ServiceCollection();

        // Register the proxy instead of the actual service,
        // so when we resolve IService, we automatically get the proxy
        services.AddSingleton(
            sp => PerformanceMonitoringProxy<IService>.Create(
                new SlowService(TimeSpan.FromMilliseconds(200)),
                threshold: TimeSpan.FromMilliseconds(10)));

        var serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetRequiredService<IService>();

        var output = OutputUtils.CaptureConsoleOutput(service.DoSomething);

        Assert.Contains("Method DoSomething took", output);
    }
}
