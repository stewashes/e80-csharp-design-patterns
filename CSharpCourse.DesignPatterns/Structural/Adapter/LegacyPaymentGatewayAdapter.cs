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
