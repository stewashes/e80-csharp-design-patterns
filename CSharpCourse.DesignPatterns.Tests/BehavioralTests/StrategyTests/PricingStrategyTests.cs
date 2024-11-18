using CSharpCourse.DesignPatterns.Behavioral.Strategy;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.StrategyTests;

public class PricingStrategyTests
{
    [Fact]
    public async Task PricingStrategy()
    {
        var standardStrategy = new StandardPricingStrategy();
        var blackFridayStrategy = new BlackFridayPricingStrategy(
            discount: 0.5m,
            discountedArticleIds: [1, 2, 3]);

        var couponCode = "ABC123";

        // Static strategies are defined at compile time
        var standardPriceCalculator = new ItemPriceCalculator(standardStrategy);
        var blackFridayPriceCalculator = new ItemPriceCalculator(blackFridayStrategy);

        var item = new Item { Id = 1, Name = "Item 1", Price = 100m };

        var standardItem = await standardPriceCalculator.ApplyPriceAsync(item, couponCode);
        var blackFridayItem = await blackFridayPriceCalculator.ApplyPriceAsync(item);

        Assert.Equal(item.Price * 0.9m, standardItem.Price);
        Assert.Equal(item.Price * 0.5m, blackFridayItem.Price);

        // Dynamic strategies can be changed at runtime
        var priceCalculator = new ItemPriceCalculator(standardStrategy);

        standardItem = await priceCalculator.ApplyPriceAsync(item, couponCode);
        Assert.Equal(item.Price * 0.9m, standardItem.Price);

        priceCalculator.SetStrategy(blackFridayStrategy);

        blackFridayItem = await priceCalculator.ApplyPriceAsync(item);
        Assert.Equal(item.Price * 0.5m, blackFridayItem.Price);

        // We can also pass a lambda for a custom strategy
        var functionalPriceCalculator = new FuncStrategyItemPriceCalculator(
            async (item, couponCode) => await Task.FromResult(item.Price * 0.8m));

        var functionalItem = await functionalPriceCalculator.ApplyPriceAsync(item);
        Assert.Equal(item.Price * 0.8m, functionalItem.Price);
    }
}
