namespace CSharpCourse.DesignPatterns.Structural.Proxy;

internal enum Item
{
    Pasta,
    Rice,
}

internal class PrimitiveWarehouse
{
    // Primitive obsession: using an int to represent a quantity
    private readonly Dictionary<Item, int> _stock = [];

    public void Restock(Item item, int quantity)
    {
        if (_stock.TryGetValue(item, out int stock))
        {
            _stock[item] = stock + quantity;
        }
        else
        {
            _stock[item] = quantity;
        }
    }

    public int GetStock(Item item) =>
        _stock.TryGetValue(item, out int stock) ? stock : 0;
}
