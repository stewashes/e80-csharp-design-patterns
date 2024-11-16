using CSharpCourse.DesignPatterns.Assignments;

namespace CSharpCourse.DesignPatterns.Tests.AssignmentTests;

public class BidirectionalListTests
{
    [Fact]
    public void BidirectionalList()
    {
        var list = new BidirectionalList<int>();

        Assert.Equal(0, list.Count);

        list.AddRange([1, 2, 3]);

        Assert.Equal(3, list.Count);

        List<int> forward = [];
        List<int> reverse = [];
        List<int> backward = [];

        foreach (var item in list)
        {
            forward.Add(item);
        }

        // This uses IEnumerable<T>.Reverse() extension method
        // which immediately enumerates the list and copies it
        // to a buffer (array), which is not very efficient if
        // we only want to iterate a few items from a huge list.
        foreach (var item in list.Reverse())
        {
            reverse.Add(item);
        }

        // Our efficient version
        foreach (var item in list.Backward())
        {
            backward.Add(item);
        }

        Assert.True(forward.SequenceEqual([1, 2, 3]));
        Assert.True(reverse.SequenceEqual([3, 2, 1]));
        Assert.True(backward.SequenceEqual([3, 2, 1]));
    }
}
