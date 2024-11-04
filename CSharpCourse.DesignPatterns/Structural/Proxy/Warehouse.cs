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
        // Our business logic requires that we can only stock an
        // amount that is a positive multiple of 20.

        // Option 1. Check each time we restock, and anywhere else
        // in our code where we refer to amounts of items in this sense.

        // Option 2. Use 1 to mean 20, 2 to mean 40 and so on.
        // This hides the actual amount, so it's harder to understand.

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

// Value proxy (we use a struct so it's stored by value)
internal struct StockQuantity
{
    private int _value; // Primitive backing field

    public int Value
    {
        readonly get => _value;
        set
        {
            if (value <= 0 || value % 20 != 0)
            {
                // Instead of throwing here we could just ignore the set
                // and log the problem, or set the value to the closest
                // allowed value
                throw new ArgumentException("Invalid value");
            }

            _value = value;
        }
    }

    public StockQuantity(int value)
    {
        Value = value;
    }

    // We can add implicit casts to and from int to improve the user experience
    public static implicit operator int(StockQuantity amount) => amount.Value;
    public static implicit operator StockQuantity(int amount) => new(amount);
}

internal class Warehouse
{
    private readonly Dictionary<Item, StockQuantity> _stock = [];

    public void Restock(Item item, StockQuantity quantity)
    {
        if (_stock.TryGetValue(item, out StockQuantity stock))
        {
            // The operators let us use the struct as if it were an int
            // by converting it implicitly to and from int
            _stock[item] = stock.Value + quantity.Value;
        }
        else
        {
            _stock[item] = quantity;
        }
    }

    public int GetStock(Item item) =>
        _stock.TryGetValue(item, out StockQuantity stock) ? stock : 0;
}
