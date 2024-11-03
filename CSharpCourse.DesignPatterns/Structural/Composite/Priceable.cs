namespace CSharpCourse.DesignPatterns.Structural.Composite;

// Generic object that can be priced
internal interface IPriceable
{

}

// Item that can have a price
internal class Item : IPriceable
{
    public decimal Price { get; set; }

    public Item(decimal price)
    {
        Price = price;
    }
}

// Box that can have multiple items or other boxes inside it
internal class Box : IPriceable
{
    public IEnumerable<IPriceable> Items { get; }

    public Box(IEnumerable<IPriceable> items)
    {
        Items = items;
    }
}
