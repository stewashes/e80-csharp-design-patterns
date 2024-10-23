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
}
