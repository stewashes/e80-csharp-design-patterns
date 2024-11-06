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
    public int Count => throw new NotImplementedException();

    public void Add(T value)
    {
        throw new NotImplementedException();
    }

    public void AddRange(IEnumerable<T> values)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> Backward()
    {
        throw new NotImplementedException();
    }

    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        throw new NotImplementedException();
    }
}
