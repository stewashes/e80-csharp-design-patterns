namespace CSharpCourse.DesignPatterns.Behavioral.Strategy;

internal interface IPricingStrategy
{
    Task<decimal> CalculatePriceAsync(Item item, string couponCode = "");
}

internal record Item
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;
}

internal class StandardPricingStrategy : IPricingStrategy
{
    public Task<decimal> CalculatePriceAsync(Item item, string couponCode = "")
    {
        if (couponCode == "ABC123")
        {
            return Task.FromResult(item.Price * 0.9m); // 10% off
        }

        return Task.FromResult(item.Price);
    }
}

internal class BlackFridayPricingStrategy : IPricingStrategy
{
    private readonly decimal _discount;
    private readonly int[] _discountedArticleIds;

    public BlackFridayPricingStrategy(decimal discount,
        IEnumerable<int> discountedArticleIds)
    {
        _discount = discount;
        _discountedArticleIds = discountedArticleIds.ToArray();
    }

    public Task<decimal> CalculatePriceAsync(Item item, string couponCode = "")
    {
        // Coupon codes are not valid during Black Friday

        return Task.FromResult(
            _discountedArticleIds.Contains(item.Id)
            ? item.Price * _discount
            : item.Price);
    }
}

internal class ItemPriceCalculator
{
    private IPricingStrategy Strategy { get; set; }

    public ItemPriceCalculator(IPricingStrategy strategy)
    {
        Strategy = strategy;
    }

    public void SetStrategy(IPricingStrategy strategy)
    {
        Strategy = strategy;
    }

    public async Task<Item> ApplyPriceAsync(Item item, string couponCode = "")
        => item with { Price = await Strategy.CalculatePriceAsync(item, couponCode) };
}

internal class FuncStrategyItemPriceCalculator
{
    private Func<Item, string, Task<decimal>> Strategy { get; set; }

    public FuncStrategyItemPriceCalculator(
        Func<Item, string, Task<decimal>> strategy)
    {
        Strategy = strategy;
    }

    public void SetStrategy(Func<Item, string, Task<decimal>> strategy)
    {
        Strategy = strategy;
    }

    public async Task<Item> ApplyPriceAsync(Item item, string couponCode = "")
        => item with { Price = await Strategy(item, couponCode) };
}
