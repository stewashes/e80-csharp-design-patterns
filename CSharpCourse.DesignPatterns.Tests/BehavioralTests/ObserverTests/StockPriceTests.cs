using CSharpCourse.DesignPatterns.Behavioral.Observer;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.ObserverTests;

public class StockPriceTests
{
    [Fact]
    public void StockPrice()
    {
        var stockPrice = new StockPrice();
        decimal? latestPrice = null;

        var observer = Observer<decimal>.Create(
            price => latestPrice = price,
            ex => { },
            () => { });

        using (stockPrice.Subscribe(observer))
        {
            stockPrice.UpdatePrice(100);
            Assert.Equal(100, latestPrice);

            stockPrice.UpdatePrice(200);
            Assert.Equal(200, latestPrice);
        }

        // The observer should have been removed from the list
        stockPrice.UpdatePrice(300);

        // The latest price should still be 200
        Assert.Equal(200, latestPrice);
    }
}
