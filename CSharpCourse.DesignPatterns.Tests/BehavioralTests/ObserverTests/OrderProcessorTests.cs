using CSharpCourse.DesignPatterns.Behavioral.Observer;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.ObserverTests;

public class OrderProcessorTests
{
    [Fact]
    public void OrderProcessor()
    {
        var processor = new OrderProcessor();
        Guid processedOrderId = Guid.Empty;

        void OnOrderProcessed(object? sender, OrderEventArgs args)
            => processedOrderId = args.OrderId;

        processor.OrderProcessed += OnOrderProcessed;

        var orderId = Guid.NewGuid();

        try
        {
            processor.ProcessOrder(orderId);

            Assert.Equal(orderId, processedOrderId);
        }
        finally
        {
            processor.OrderProcessed -= OnOrderProcessed;
        }

        var orderId2 = Guid.NewGuid();
        processor.ProcessOrder(orderId2);

        Assert.Equal(orderId, processedOrderId);
    }

    [Fact]
    public async Task OrderProcessorAsync()
    {
        var processor = new OrderProcessor();
        Guid processedOrderId = Guid.Empty;

        async Task OnOrderProcessedAsync(OrderEventArgs args)
        {
            await Task.Delay(100);
            processedOrderId = args.OrderId;
        }

        processor.OrderProcessedAsync += OnOrderProcessedAsync;

        var orderId = Guid.NewGuid();

        try
        {
            await processor.ProcessOrderAsync(orderId);

            Assert.Equal(orderId, processedOrderId);
        }
        finally
        {
            processor.OrderProcessedAsync -= OnOrderProcessedAsync;
        }

        var orderId2 = Guid.NewGuid();
        await processor.ProcessOrderAsync(orderId2);

        Assert.Equal(orderId, processedOrderId);
    }

    [Fact]
    public async Task OrderProcessorAsyncParallel()
    {
        var processor = new OrderProcessor();
        Guid processedOrderId = Guid.Empty;

        async Task OnOrderProcessedAsync(OrderEventArgs args)
        {
            await Task.Delay(100);
            processedOrderId = args.OrderId;
        }

        processor.OrderProcessedAsync += OnOrderProcessedAsync;

        var orderId = Guid.NewGuid();

        try
        {
            await processor.ProcessOrderAsyncParallel(orderId);

            Assert.Equal(orderId, processedOrderId);
        }
        finally
        {
            processor.OrderProcessedAsync -= OnOrderProcessedAsync;
        }

        var orderId2 = Guid.NewGuid();
        await processor.ProcessOrderAsyncParallel(orderId2);

        Assert.Equal(orderId, processedOrderId);
    }
}
