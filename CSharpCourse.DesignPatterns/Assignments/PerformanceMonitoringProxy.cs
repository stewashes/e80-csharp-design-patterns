using System.Diagnostics;
using System.Reflection;

namespace CSharpCourse.DesignPatterns.Assignments;

internal interface IService
{
    void DoSomething();
    Task DoSomethingAsync();
    Task<bool> GetResultAsync();
}

internal class SlowService : IService
{
    private readonly TimeSpan _delay;

    public SlowService(TimeSpan delay)
    {
        _delay = delay;
    }

    public void DoSomething()
    {
        Thread.Sleep(_delay);
    }

    public async Task DoSomethingAsync()
    {
        await Task.Delay(_delay);
    }

    public async Task<bool> GetResultAsync()
    {
        await Task.Delay(_delay);
        return true;
    }
}

internal class PerformanceMonitoringProxy<T> where T : class
{
    public static T Create(T decorated, TimeSpan threshold)
    {
        // Implement this
        throw new NotImplementedException();
    }
}
