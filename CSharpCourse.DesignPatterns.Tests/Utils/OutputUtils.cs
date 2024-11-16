namespace CSharpCourse.DesignPatterns.Tests.Utils;

internal static class OutputUtils
{
    // Thread-safe lock for the console
    private static readonly SemaphoreSlim _consoleLock = new(1);

    internal static string CaptureConsoleOutput(Action action)
    {
        _consoleLock.Wait();

        var originalOutput = Console.Out;

        try
        {
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            action();
            return stringWriter.ToString().Trim();
        }
        finally
        {
            Console.SetOut(originalOutput);
            _consoleLock.Release();
        }
    }

    internal static async Task<string> CaptureConsoleOutputAsync(Func<Task> action)
    {
        await _consoleLock.WaitAsync();

        var originalOutput = Console.Out;

        try
        {
            using var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);
            await action();
            return stringWriter.ToString().Trim();
        }
        finally
        {
            Console.SetOut(originalOutput);
            _consoleLock.Release();
        }
    }
}
