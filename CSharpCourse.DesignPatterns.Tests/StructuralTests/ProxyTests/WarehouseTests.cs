using CSharpCourse.DesignPatterns.Structural.Proxy;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.ProxyTests;

public class WarehouseTests
{
    [Fact]
    public void PrimitiveObsession()
    {
        var warehouse = new PrimitiveWarehouse();

        warehouse.Restock(Item.Pasta, 20);

        // Does not throw
        warehouse.Restock(Item.Rice, 10);

        Assert.Equal(20, warehouse.GetStock(Item.Pasta));
        Assert.Equal(10, warehouse.GetStock(Item.Rice));
    }

    [Fact]
    public void ValueProxy()
    {
        var warehouse = new Warehouse();

        warehouse.Restock(Item.Pasta, 20);

        Assert.Throws<ArgumentException>(
            () => warehouse.Restock(Item.Rice, 10));

        Assert.Equal(20, warehouse.GetStock(Item.Pasta));
        Assert.Equal(0, warehouse.GetStock(Item.Rice));
    }
}
