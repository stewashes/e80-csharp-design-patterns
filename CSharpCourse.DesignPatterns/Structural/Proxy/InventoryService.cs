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
