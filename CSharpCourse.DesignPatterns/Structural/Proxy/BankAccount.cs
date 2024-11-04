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

// We don't change the existing class, so we're not breaking the
// Open/Closed Principle.
internal class ProtectedBankAccount : IBankAccount
{
    // We create a new instance of the BankAccount class,
    // we're not decorating it.
    private readonly BankAccount _account;

    public ProtectedBankAccount(decimal balance)
    {
        _account = new(balance);
    }

    public decimal Balance
    {
        get => _account.Balance;
        set => throw new UnauthorizedAccessException(
            "You cannot change the balance of this account");
    }

    public void Transfer(decimal amount, IBankAccount recipient)
    {
        if (recipient == this)
        {
            throw new InvalidOperationException(
                "You cannot transfer money to the same account");
        }

        if (amount <= 0)
        {
            throw new InvalidOperationException(
                "The amount must be a positive number");
        }

        if (amount > 1_000)
        {
            throw new InvalidOperationException(
                "You cannot transfer more than 1,000 units at a time");
        }

        _account.Transfer(amount, recipient);
    }
}
