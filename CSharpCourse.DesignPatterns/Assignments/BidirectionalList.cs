using System.Collections;
namespace CSharpCourse.DesignPatterns.Assignments;

internal interface IBidirectionalList<T> : IEnumerable<T>
{
    int Count { get; }
    void Add(T value);
    void AddRange(IEnumerable<T> values);
    IEnumerable<T> Backward();
}

internal class BidirectionalList<T> : IBidirectionalList<T>
{
    private readonly LinkedList<T> _list = new();

    public int Count => _list.Count;

    public void Add(T value)
    {
        _list.AddLast(value);
    }

    public void AddRange(IEnumerable<T> values)
    {
        foreach (var value in values)
        {
            Add(value);
        }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<T> Backward()
    {
        var node = _list.Last;

        while (node is not null)
        {
            yield return node.Value;
            node = node.Previous;
        }
    }
}
