namespace CSharpCourse.DesignPatterns.Structural.Proxy;

// The bank account is just a textbook example, in reality this
// could be any class that we want to restrict access to.

internal interface IBankAccount
{
    decimal Balance { get; set; }
    void Transfer(decimal amount, IBankAccount recipient);
}

internal class BankAccount : IBankAccount
{
    public decimal Balance { get; set; }

    public BankAccount(decimal balance)
    {
        Balance = balance;
    }

    public void Transfer(decimal amount, IBankAccount recipient)
    {
        recipient.Balance += amount;
        Balance -= amount;
    }
}
