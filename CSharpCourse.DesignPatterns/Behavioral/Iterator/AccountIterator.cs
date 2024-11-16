namespace CSharpCourse.DesignPatterns.Behavioral.Iterator;

internal class Account
{
    public decimal Balance { get; set; } = 0;
    public Account[] SubAccounts { get; set; } = [];

    #region Duck Typing
    // If we do this, there must be only one way to iterate over the object
    public IAccountIterator GetEnumerator() => new AccountLevelIterator(this, 3);
    #endregion
}

internal interface IAccountIterator
{
    public Account? Current { get; }

    // Returns true if it could move to the next one
    bool MoveNext();

    void Reset();
}

internal class AccountLevelIterator : IAccountIterator
{
    private readonly Account _root;
    private Account[] _flattenedCollection = [];
    private int _index = 0;

    public Account? Current { get; private set; }
    public int Level { get; private set; }

    public AccountLevelIterator(Account root, int level)
    {
        _root = root;
        Level = level;

        Reset();
    }

    public bool MoveNext()
    {
        if (_index < _flattenedCollection.Length)
        {
            Current = _flattenedCollection[_index++];
            return true;
        }

        return false;
    }

    public void Reset()
    {
        _flattenedCollection = Flatten(_root).ToArray();
        Current = _flattenedCollection.FirstOrDefault();
    }

    private IEnumerable<Account> Flatten(Account account, int currentLevel = 1)
    {
        // If the target level is 1, just return the root account
        if (Level == 1)
        {
            return [account];
        }

        // If we're 1 level above the target level, return the sub accounts
        if ((currentLevel + 1) == Level)
        {
            return account.SubAccounts;
        }

        // If we're not there yet, flatten recursively
        return account.SubAccounts.SelectMany(a => Flatten(a, currentLevel + 1));
    }
}

#region Method iterator
internal static class AccountExtensions
{
    // IEnumerator<T> is the low-level interface that allows to iterate
    // over a collection, while IEnumerable<T> is the high-level interface
    // that defines a GetEnumerator method that returns an IEnumerator<T>.

    // The IEnumerable<T> interface is the foundation of LINQ's query
    // capabilities. It allows to access objects sequentially, without
    // exposing the underlying data structure.
    public static IEnumerable<Account> AtLevel(this Account account, int level)
    {
        var iterator = new AccountLevelIterator(account, level);

        while (iterator.MoveNext())
        {
            if (iterator.Current is not null)
            {
                yield return iterator.Current;
            }
        }
    }
}
#endregion
