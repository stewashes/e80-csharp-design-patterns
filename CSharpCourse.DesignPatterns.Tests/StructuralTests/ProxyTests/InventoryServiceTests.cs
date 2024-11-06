using CSharpCourse.DesignPatterns.Structural.Proxy;
using CSharpCourse.DesignPatterns.Tests.Utils;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.ProxyTests;

public class InventoryServiceTests
{
    [Fact]
    public void DispatchProxy()
    {
        var inventoryService = new InventoryService();
        var proxy = LoggingProxy<IInventoryService>.Create(inventoryService);

        // Proxy will log the method calls
        var output = OutputUtils.CaptureConsoleOutput(
            () => proxy.UpdateStock("Laptop", 10));

        Assert.Contains("Invoking UpdateStock with arguments: Laptop, 10", output);
    }
}
