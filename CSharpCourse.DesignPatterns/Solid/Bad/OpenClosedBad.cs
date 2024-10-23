namespace CSharpCourse.DesignPatterns.Solid.Bad;

internal record Invoice
{
    public required decimal Amount { get; init; }
    public required InvoiceType InvoiceType { get; init; }
    
    public decimal GetInvoiceDiscount()
    {
        return InvoiceType switch
        {
            InvoiceType.ProposedInvoice => Amount * 0.15m,
            InvoiceType.FinalInvoice => Amount * 0.25m,
            InvoiceType.RecurringInvoice => Amount * 0.3m,
            _ => 0
        };
    }
}

internal enum InvoiceType
{
    ProposedInvoice,
    FinalInvoice,
    RecurringInvoice
};
