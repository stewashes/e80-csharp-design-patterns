using CSharpCourse.DesignPatterns.Structural.Adapter;
using System.Collections.Specialized;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.AdapterTests;

public class ObservableCollectionAdapterTests
{
    class ProductModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }

    [Fact]
    public void CollectionAdapter()
    {
        var enumerable = new List<ProductModel>();
        var adapter = new ObservableCollectionAdapter<ProductModel>(enumerable);

        // The list of changes that occur in the collection
        var collectionChanges = new List<NotifyCollectionChangedEventArgs>();

        // When the collection changes, add the event to the list
        adapter.CollectionChanged += (s, e) => collectionChanges.Add(e);

        var newProducts = new[]
        {
            new ProductModel { Id = 1, Name = "Product 1" },
            new ProductModel { Id = 2, Name = "Product 2" }
        };

        adapter.ReplaceAll(newProducts); // Performs: Clear + one Add for each item

        Assert.Equal(2, adapter.Count);
        Assert.Equal(3, collectionChanges.Count);
        Assert.Equal(NotifyCollectionChangedAction.Reset, collectionChanges[0].Action);
        Assert.Equal(NotifyCollectionChangedAction.Add, collectionChanges[1].Action);
        Assert.Equal(NotifyCollectionChangedAction.Add, collectionChanges[2].Action);
    }
}
