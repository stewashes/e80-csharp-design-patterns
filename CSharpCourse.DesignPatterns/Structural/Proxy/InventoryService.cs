using System.Reflection;

namespace CSharpCourse.DesignPatterns.Structural.Proxy;

internal interface IInventoryService
{
    void UpdateStock(string product, int quantity);
}

internal class InventoryService : IInventoryService
{
    public void UpdateStock(string product, int quantity)
    {
        Console.WriteLine($"Stock updated: {product}, Quantity: {quantity}");
    }
}

// Logging proxy with DispatchProxy
// (another interesting use case is performance monitoring)
internal class LoggingProxy<T> : DispatchProxy where T : class
{
    private T? _decorated;

    // In this case, we pass the original object to the proxy.
    // This is a factory method that creates the proxy object.
    public static T Create(T decorated)
    {
        // When we call the Create method, .NET will dynamically create
        // an instance of a new runtime-defined type that implements both
        // T and LoggingProxy<T>, and internally routes calls to the
        // Invoke method.
        object proxy = Create<T, LoggingProxy<T>>();
        ((LoggingProxy<T>)proxy)._decorated = decorated;
        return (T)proxy;
    }

    // DispatchProxy will intercept the method calls and call this method.
    protected override object? Invoke(MethodInfo? targetMethod, object?[]? args)
    {
        ArgumentNullException.ThrowIfNull(targetMethod);

        Console.WriteLine($"Invoking {targetMethod.Name} with arguments: {string.Join(", ", args ?? [])}");

        // Call the method on the original object
        var result = targetMethod.Invoke(_decorated, args);

        Console.WriteLine($"Completed {targetMethod.Name}");
        return result;
    }
}
