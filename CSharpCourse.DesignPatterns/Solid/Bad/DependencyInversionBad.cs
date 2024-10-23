namespace CSharpCourse.DesignPatterns.Solid.Bad;

internal record Order
{
    public Guid Id { get; init; }
    // Other order details...
}

internal class OrderService
{
    // OrderService depends on the concrete FileLogger class,
    // only logging to a file is supported (no console, database, aggregators, ...)
    private readonly FileLogger _logger;

    public OrderService(FileLogger logger)
    {
        _logger = logger;
    }
    
    public async Task PlaceOrderAsync(Order order)
    {
        // Logic to place order
        await _logger.LogAsync($"Order {order.Id} placed at {DateTime.Now}");
    }
}

internal class FileLogger
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
