namespace CSharpCourse.DesignPatterns.Structural.Adapter;

// Scenario: Adapting a legacy payment gateway to work with
// a modern payment interface

// NOTE: The adapter could also work the other way around,
// adapting a modern interface to work with a legacy system.

#region New interface and classes
internal record PaymentRequest
{
    public required string CardNumber { get; init; }
    public decimal Amount { get; init; }
    public required string Currency { get; init; }
}

internal record PaymentResult
{
    public bool Success { get; init; }
    public required string TransactionId { get; init; }
}

internal record RefundRequest
{
    public required string TransactionId { get; init; }
    public decimal Amount { get; init; }
}

internal record RefundResult
{
    public bool Success { get; init; }
}

internal interface IPaymentProcessor
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request);
    Task<RefundResult> ProcessRefundAsync(RefundRequest request);
}
#endregion

#region Legacy payment gateway we need to adapt
internal class LegacyPaymentGateway
{
    public bool MakePayment(string cardNumber, decimal amount, string currency)
    {
        // Legacy implementation
        return true;
    }

    public bool RefundPayment(string transactionId, decimal amount)
    {
        // Legacy implementation
        return true;
    }
}
#endregion

#region Class adapter using inheritance
// Class adapters can be more rigid, since they are tied to their parent class,
// but they can reuse behavior from the parent class. If the parent class
// changes, the adapter often needs to change as well.
internal class LegacyPaymentGatewayClassAdapter : LegacyPaymentGateway, IPaymentProcessor
{
    public Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        var success = MakePayment(request.CardNumber, request.Amount, request.Currency);
        var result = new PaymentResult { Success = success, TransactionId = Guid.NewGuid().ToString() };
        return Task.FromResult(result);
    }

    public Task<RefundResult> ProcessRefundAsync(RefundRequest request)
    {
        var success = RefundPayment(request.TransactionId, request.Amount);
        var result = new RefundResult { Success = success };
        return Task.FromResult(result);
    }
}
#endregion

#region Object adapter using composition
// Object adapters can be more flexible, since they effectively wrap the adaptee
// and translate the calls, so they are more flexible and resilient to changes.
internal class LegacyPaymentGatewayObjectAdapter : IPaymentProcessor
{
    private readonly LegacyPaymentGateway _legacyPaymentGateway;

    public LegacyPaymentGatewayObjectAdapter(LegacyPaymentGateway legacyPaymentGateway)
    {
        _legacyPaymentGateway = legacyPaymentGateway;
    }

    public Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request)
    {
        var success = _legacyPaymentGateway.MakePayment(request.CardNumber, request.Amount, request.Currency);
        var result = new PaymentResult { Success = success, TransactionId = Guid.NewGuid().ToString() };
        return Task.FromResult(result);
    }

    public Task<RefundResult> ProcessRefundAsync(RefundRequest request)
    {
        var success = _legacyPaymentGateway.RefundPayment(request.TransactionId, request.Amount);
        var result = new RefundResult { Success = success };
        return Task.FromResult(result);
    }
}
#endregion
