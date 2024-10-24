namespace CSharpCourse.DesignPatterns.Solid.Good;

internal record Order
{
    public Guid Id { get; init; }
    // Other order details...
}

internal class OrderService(ILogger logger)
{
    // OrderService depends on the abstract ILogger interface,
    // so any type of logging can be used, as long as they implement
    // the ILogger interface
    private readonly ILogger _logger = logger;

    public async Task PlaceOrderAsync(Order order)
    {
        // Logic to place order
        await _logger.LogAsync($"Order {order.Id} placed at {DateTime.Now}");
    }
}

internal interface ILogger
{
    Task LogAsync(string message);
}

internal class FileLogger : ILogger
{
    private readonly string _logFileName;

    public FileLogger(string logFileName)
    {
        _logFileName = logFileName;
    }
    
    public async Task LogAsync(string message)
    {
        // Write log message to a file
        await File.AppendAllTextAsync(_logFileName, message + Environment.NewLine);
    }
}

internal class ConsoleLogger : ILogger
{
    public Task LogAsync(string message)
    {
        // Write log message to the console
        Console.WriteLine(message);
        return Task.CompletedTask;
    }
}

internal class InMemoryLogger : ILogger
{
    public IList<string> Log { get; } = new List<string>();

    public Task LogAsync(string message)
    {
        Log.Add(message);
        return Task.CompletedTask;
    }
}
