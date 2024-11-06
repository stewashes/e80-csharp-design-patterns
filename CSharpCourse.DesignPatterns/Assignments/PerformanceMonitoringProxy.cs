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

internal class PerformanceMonitoringProxy<T> : DispatchProxy where T : class
{
    private T? _decorated;
    private TimeSpan _threshold = TimeSpan.FromSeconds(1);

    public static T Create(T decorated, TimeSpan threshold)
    {
        object proxy = Create<T, PerformanceMonitoringProxy<T>>();
        ((PerformanceMonitoringProxy<T>)proxy)._decorated = decorated;
        ((PerformanceMonitoringProxy<T>)proxy)._threshold = threshold;
        return (T)proxy;
    }

    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        ArgumentNullException.ThrowIfNull(targetMethod);

        if (targetMethod.ReturnType == typeof(Task))
        {
            return InvokeAsync(targetMethod, args);
        }
        else if (targetMethod.ReturnType.IsGenericType && targetMethod.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
        {
            // Get the type of T in Task<T> and make a dynamic call
            // to InvokeAsyncWithResult<T>
            var returnType = targetMethod.ReturnType.GetGenericArguments()[0];
            return typeof(PerformanceMonitoringProxy<T>)
                .GetMethod(nameof(InvokeAsyncWithResult), BindingFlags.NonPublic | BindingFlags.Instance)!
                .MakeGenericMethod(returnType)
                .Invoke(this, [targetMethod, args]);

            // CAREFUL: Using reflection in performance-critical code can
            // have a significant impact on performance. Only use this when
            // doing performance monitoring, not in production code.

            // NOTE: We could use DynamicMethod for better performance.
        }
        // NOTE: The ValueTask and ValueTask<T> cases are not covered, but
        // they can be implemented in a similar way.
        else
        {
            var startTime = Stopwatch.GetTimestamp();

            // Synchronous execution
            var result = targetMethod.Invoke(_decorated, args);
            var delta = Stopwatch.GetElapsedTime(startTime);

            if (delta > _threshold)
            {
                Console.WriteLine($"Method {targetMethod.Name} took {delta.TotalMilliseconds} ms to execute");
            }

            return result;
        }
    }

    private async Task InvokeAsync(MethodInfo targetMethod, object?[]? args)
    {
        var startTime = Stopwatch.GetTimestamp();

        var task = (Task)targetMethod.Invoke(_decorated, args)!;
        await task.ConfigureAwait(false);

        var delta = Stopwatch.GetElapsedTime(startTime);
        if (delta > _threshold)
        {
            Console.WriteLine($"Method {targetMethod.Name} took {delta.TotalMilliseconds} ms to execute");
        }
    }

    private async Task<TResult> InvokeAsyncWithResult<TResult>(MethodInfo targetMethod, object?[]? args)
    {
        var startTime = Stopwatch.GetTimestamp();

        var task = (Task<TResult>)targetMethod.Invoke(_decorated, args)!;
        var result = await task.ConfigureAwait(false);

        var delta = Stopwatch.GetElapsedTime(startTime);
        if (delta > _threshold)
        {
            Console.WriteLine($"Method {targetMethod.Name} took {delta.TotalMilliseconds} ms to execute");
        }

        return result;
    }
}
