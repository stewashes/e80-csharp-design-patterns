namespace CSharpCourse.DesignPatterns.Tests.SolidTests;

public class DependencyInversionTests
{
    [Fact]
    public async Task Bad()
    {
        var orderId = Guid.NewGuid();
        var logFileName = Path.GetRandomFileName();
        var order = new Solid.Bad.Order { Id = orderId };
        
        // Only the file logger is supported
        var fileLogger = new Solid.Bad.FileLogger(logFileName);
        var orderService = new Solid.Bad.OrderService(fileLogger);
        await orderService.PlaceOrderAsync(order);
        
        var fileContents = await File.ReadAllTextAsync(logFileName);
        Assert.Contains(orderId.ToString(), fileContents);
    }

    [Fact]
    public async Task Good()
    {
        var orderId = Guid.NewGuid();
        var logFileName = Path.GetRandomFileName();
        var order = new Solid.Good.Order { Id = orderId };
        
        // File logger
        var fileLogger = new Solid.Good.FileLogger(logFileName);
        var orderService = new Solid.Good.OrderService(fileLogger);
        await orderService.PlaceOrderAsync(order);
        
        var fileContents = await File.ReadAllTextAsync(logFileName);
        Assert.Contains(orderId.ToString(), fileContents);
        
        // Console logger
        var consoleLogger = new Solid.Good.ConsoleLogger();
        orderService = new Solid.Good.OrderService(consoleLogger);
        await orderService.PlaceOrderAsync(order);

        // In-memory logger
        var inMemoryLogger = new Solid.Good.InMemoryLogger();
        orderService = new Solid.Good.OrderService(inMemoryLogger);
        await orderService.PlaceOrderAsync(order);

        Assert.Contains(orderId.ToString(), inMemoryLogger.Log[^1]);
    }
}
