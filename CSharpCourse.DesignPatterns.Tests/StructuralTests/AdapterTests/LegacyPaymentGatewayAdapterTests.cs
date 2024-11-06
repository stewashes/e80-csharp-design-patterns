using CSharpCourse.DesignPatterns.Structural.Adapter;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.AdapterTests;

public class LegacyPaymentGatewayAdapterTests
{
    [Fact]
    public async Task ClassAdapter()
    {
        var paymentProcessor = new LegacyPaymentGatewayClassAdapter();
        var paymentRequest = new PaymentRequest
        {
            CardNumber = "4111111111111111",
            Amount = 99.99m,
            Currency = "USD"
        };

        var result = await paymentProcessor.ProcessPaymentAsync(paymentRequest);

        Assert.True(result.Success);
        Assert.NotNull(result.TransactionId);
        Assert.NotEmpty(result.TransactionId);
    }

    [Fact]
    public async Task ObjectAdapter()
    {
        var legacyPaymentGateway = new LegacyPaymentGateway();
        var paymentProcessor = new LegacyPaymentGatewayObjectAdapter(legacyPaymentGateway);
        var refundRequest = new RefundRequest
        {
            TransactionId = "123456",
            Amount = 99.99m
        };

        var result = await paymentProcessor.ProcessRefundAsync(refundRequest);

        Assert.True(result.Success);
    }
}
