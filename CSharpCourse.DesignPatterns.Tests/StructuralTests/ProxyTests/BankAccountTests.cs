using CSharpCourse.DesignPatterns.Structural.Proxy;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.ProxyTests;

public class BankAccountTests
{
    [Fact]
    public void BankAccount()
    {
        var mario = new BankAccount(1_000m);
        var luigi = new BankAccount(500m);

        // Nothing prevents us from doing this
        mario.Transfer(100, mario);

        // ...or this
        mario.Balance = 1_000_000_000;
        Assert.Equal(1_000_000_000, mario.Balance);

        // Also, we might want to limit the amount that is sent,
        // or notify transactions above a certain threshold.
        mario.Transfer(1_000_000_000, luigi);
    }
}
