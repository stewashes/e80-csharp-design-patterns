namespace CSharpCourse.DesignPatterns.Solid.Good;

internal abstract record Invoice
{
    public required decimal Amount { get; init; }
    
    // Here we can calculate the base discount, which could depend
    // on factors that are common to all types of invoices (e.g., coupons)
    internal virtual decimal GetInvoiceDiscount() => 0;
}

internal record ProposedInvoice : Invoice
{
    internal override decimal GetInvoiceDiscount() 
        => base.GetInvoiceDiscount() + Amount * 0.15m;
}

internal record FinalInvoice : Invoice
{
    internal override decimal GetInvoiceDiscount()
        => base.GetInvoiceDiscount() + Amount * 0.25m;
}

internal record RecurringInvoice : Invoice
{
    internal override decimal GetInvoiceDiscount() 
        => base.GetInvoiceDiscount() + Amount * 0.3m;
}
