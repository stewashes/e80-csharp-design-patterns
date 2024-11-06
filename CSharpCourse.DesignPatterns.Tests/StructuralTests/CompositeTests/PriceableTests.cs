using CSharpCourse.DesignPatterns.Structural.Composite;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.CompositeTests;

public class PriceableTests
{
    [Fact]
    public void PriceableTree()
    {
        // Without the composite pattern, the client code would look like this

        // This is the tree-like structure
        var container = new Box(
        [
            new Item(24.99m),
            new Item(19.99m),
            new Box(
            [
                new Item(5.49m),
                new Item(119.99m)
            ])
        ]);

        decimal CalculatePrice(IPriceable priceable)
        {
            // We violate the open-close principle here. If we decide to add a new
            // type that can be added to a box, we need to change this method.
            return priceable switch
            {
                Item item => item.Price,
                Box box => box.Items.Sum(CalculatePrice),
                _ => throw new NotImplementedException(),
            };
        }

        // We want to calculate the overall price of the items
        var price = CalculatePrice(container);

        Assert.Equal(170.46m, price);
    }

    [Fact]
    public void PriceableTreeComposite()
    {
        // This is the tree-like structure
        var container = new Box(
        [
            new Item(24.99m),
            new Item(19.99m),
            new Box(
            [
                new Item(5.49m),
                new Item(119.99m)
            ])
        ]);

        // We want to calculate the overall price of the items
        var price = container.CalculatePrice();

        Assert.Equal(170.46m, price);
    }
}
