﻿namespace CSharpCourse.DesignPatterns.Tests.Utils;

internal static class OutputUtils
{
    internal static string CaptureConsoleOutput(Action action)
    {
        var originalOutput = Console.Out;
        
        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        action();
        Console.SetOut(originalOutput);
        return stringWriter.ToString().Trim();
    }

    internal static async Task<string> CaptureConsoleOutputAsync(Func<Task> action)
    {
        var originalOutput = Console.Out;

        using var stringWriter = new StringWriter();
        Console.SetOut(stringWriter);
        await action();
        Console.SetOut(originalOutput);
        return stringWriter.ToString().Trim();
    }
}
