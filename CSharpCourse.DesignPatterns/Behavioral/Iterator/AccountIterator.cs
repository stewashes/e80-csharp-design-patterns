namespace CSharpCourse.DesignPatterns.Behavioral.Iterator;

internal class Account
{
    public decimal Balance { get; set; } = 0;
    public Account[] SubAccounts { get; set; } = [];
}
