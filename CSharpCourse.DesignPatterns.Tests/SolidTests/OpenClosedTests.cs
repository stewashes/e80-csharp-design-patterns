namespace CSharpCourse.DesignPatterns.Tests.SolidTests;

public class OpenClosedTests
{
    [Fact]
    public void Bad()
    {
        var proposedInvoice = new Solid.Bad.Invoice
        {
            Amount = 100m,
            InvoiceType = Solid.Bad.InvoiceType.ProposedInvoice
        };

        var finalInvoice = new Solid.Bad.Invoice
        {
            Amount = 100m,
            InvoiceType = Solid.Bad.InvoiceType.FinalInvoice
        };
        
        var recurringInvoice = new Solid.Bad.Invoice
        {
            Amount = 100m,
            InvoiceType = Solid.Bad.InvoiceType.RecurringInvoice
        };
        
        Assert.Equal(15m, proposedInvoice.GetInvoiceDiscount());
        Assert.Equal(25m, finalInvoice.GetInvoiceDiscount());
        Assert.Equal(30m, recurringInvoice.GetInvoiceDiscount());
    }
    
    [Fact]
    public void Good()
    {
        var proposedInvoice = new Solid.Good.ProposedInvoice
        {
            Amount = 100m
        };

        var finalInvoice = new Solid.Good.FinalInvoice
        {
            Amount = 100m
        };
        
        var recurringInvoice = new Solid.Good.RecurringInvoice
        {
            Amount = 100m
        };
        
        Assert.Equal(15m, proposedInvoice.GetInvoiceDiscount());
        Assert.Equal(25m, finalInvoice.GetInvoiceDiscount());
        Assert.Equal(30m, recurringInvoice.GetInvoiceDiscount());
    }
}
