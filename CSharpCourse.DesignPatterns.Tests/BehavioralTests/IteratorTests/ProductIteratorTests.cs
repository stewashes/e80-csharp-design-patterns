using CSharpCourse.DesignPatterns.Behavioral.Iterator;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.IteratorTests;

public class ProductIteratorTests
{
    [Fact]
    public void ProductIterator()
    {
        List<Product> products =
        [
            new Product { Name = "Product A", Price = 9.99m },
            new Product { Name = "Product B", Price = 14.99m },
            new Product { Name = "Product C", Price = 19.99m }
        ];

        var iterator = new ProductIterator(products);

        var productNames = new List<string>();
        while (iterator.HasNext)
        {
            productNames.Add(iterator.Current.Name);
            iterator.Next();
        }

        Assert.Equal([ "Product A", "Product B", "Product C" ], productNames);
    }

    [Fact]
    public async Task AsyncProductIterator()
    {
        var productData = Enumerable.Range(1, 100)
            .Select(i => new Product { Name = $"Product {i}", Price = i * 0.99m });

        var repository = new ProductRepository(productData.ToArray());

        // Let's only take the first 10
        var count = 0;

        await foreach (var product in repository.GetProductsAsync())
        {
            count++;

            if (count == 10)
            {
                break;
            }
        }

        // Make sure the other ones are not loaded
        Assert.Equal(10, repository.LoadedProducts);
    }
}
