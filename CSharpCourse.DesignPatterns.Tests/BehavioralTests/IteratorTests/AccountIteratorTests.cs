using CSharpCourse.DesignPatterns.Behavioral.Iterator;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.IteratorTests;

public class AccountIteratorTests
{
    [Fact]
    public void ObjectIterator()
    {
        var masterAccount = new Account
        {
            Balance = 1_000_000,
            SubAccounts =
            [
                new Account { Balance = 500 },
                new Account
                {
                    Balance = 900,
                    SubAccounts =
                    [
                        new Account { Balance = 50 },
                        new Account { Balance = 90 }
                    ]
                },
                new Account
                {
                    Balance = 1_200,
                    SubAccounts =
                    [
                        new Account { Balance = 1_800 }
                    ]
                }
            ]
        };

        var level = 3;
        var iterator = new AccountLevelIterator(masterAccount, level);
        var totalBalance = 0m;

        while (iterator.MoveNext())
        {
            if (iterator.Current is not null)
            {
                totalBalance += iterator.Current.Balance;
            }
        }

        Assert.Equal(1940, totalBalance);

        totalBalance = 0m;

        foreach (var account in masterAccount.AtLevel(3))
        {
            totalBalance += account.Balance;
        }

        Assert.Equal(1940, totalBalance);

        totalBalance = masterAccount
            .AtLevel(3)
            .Sum(account => account.Balance);

        Assert.Equal(1940, totalBalance);
    }
}
