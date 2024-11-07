namespace CSharpCourse.DesignPatterns.Behavioral.Iterator;

internal record Product
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public decimal Price { get; set; }
}

// Covariant interface
// Covariance means that you can use a more derived type than
// originally specified (e.g., IIterator<DerivedProduct> where
// IIterator<Product> is expected).
internal interface IIterator<out T>
{
    T Current { get; }
    bool HasNext { get; }
    void Next();
}

internal class ProductIterator : IIterator<Product>
{
    private readonly List<Product> _products;
    private int _currentIndex;

    public ProductIterator(List<Product> products)
    {
        _products = products;
        _currentIndex = 0;
    }

    public Product Current => _products[_currentIndex];

    public bool HasNext => _currentIndex < _products.Count;

    public void Next()
    {
        _currentIndex++;
    }
}

internal record ProductDetails
{
    public Guid Id { get; set; }
    public string? Description { get; set; }
}

internal interface IProductRepository
{
    IAsyncEnumerable<ProductDetails> GetProductsAsync();
}

internal class ProductRepository : IProductRepository
{
    // Imagine this is a database or a file
    private readonly Product[] _products;
    private readonly Dictionary<Guid, ProductDetails> _productDetails = [];

    public ProductRepository(Product[] products)
    {
        _products = products;
    }

    public int LoadedProducts => _productDetails.Count;

    public async IAsyncEnumerable<ProductDetails> GetProductsAsync()
    {
        foreach (var product in _products)
        {
            if (!_productDetails.TryGetValue(product.Id, out var productDetails))
            {
                productDetails = await FetchProductDetailsAsync(product);
                _productDetails[product.Id] = productDetails;
            }

            yield return productDetails;
        }
    }

    private async Task<ProductDetails> FetchProductDetailsAsync(Product product)
    {
        // Imagine this is a database or a file
        await Task.Delay(100);
        return new ProductDetails 
        { 
            Id = product.Id, 
            Description = $"Description of {product.Name}"
        };
    }
}
