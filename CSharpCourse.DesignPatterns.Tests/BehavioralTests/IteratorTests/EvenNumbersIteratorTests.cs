using CSharpCourse.DesignPatterns.Behavioral.Iterator;
using System.Numerics;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.IteratorTests;

public class EvenNumbersIteratorTests
{
    [Fact]
    public void EvenNumbersIterator()
    {
        var numbers = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var evenSum = 0;

        // IEnumerator<T> is the low-level interface that allows to iterate
        // over a collection, while IEnumerable<T> is the high-level interface
        // that defines a GetEnumerator method that returns an IEnumerator<T>.
        var iterator = numbers.OnlyEven().GetEnumerator();

        while (iterator.MoveNext())
        {
            evenSum += iterator.Current;
        }

        Assert.Equal(30, evenSum);

        evenSum = 0;

        // The IEnumerable<T> interface allows to access objects sequentially,
        // without exposing the underlying data structure. It is the foundation
        // of LINQ's query capabilities.
        foreach (var number in numbers.OnlyEven())
        {
            evenSum += number;
        }

        Assert.Equal(30, evenSum);

        // Using LINQ
        evenSum = numbers
            .OnlyEven()
            .Sum();

        Assert.Equal(30, evenSum);
    }

    // Method that returns a set of test data
    // (the number of tests varies at runtime)
    public static IEnumerable<object[]> GetNumberArrays()
    {
        yield return new object[] { new long[] { 1, 2, 3, 4, 5, 6, }, 12L };
        yield return new object[] { new BigInteger[] { 1, 2, 3, 4, 5, 6 }, new BigInteger(12) };
        yield return new object[] { new decimal[] {1, 2, 3, 4, 5, 6 }, 12M };
    }

    [Theory]
    [MemberData(nameof(GetNumberArrays))]
    public void EvenNumbersGenericIterator<T>(IEnumerable<T> numbers, T expectedSum)
        where T : INumber<T>
    {
        // We cannot use .Sum() since it is not available for INumber<T>
        // (e.g., IEnumerable<BigInteger> does not implement it)
        var evenSum = numbers
            .OnlyEvenGeneric()
            .Aggregate(T.Zero, (acc, i) => acc + i);

        Assert.Equal(expectedSum, evenSum);
    }
}
