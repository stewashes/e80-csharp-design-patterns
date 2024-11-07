using System.Numerics;

namespace CSharpCourse.DesignPatterns.Behavioral.Iterator;

internal static class EnumerableExtensions
{
    public static IEnumerable<int> OnlyEven(this IEnumerable<int> array)
    {
        foreach (var i in array)
        {
            if (i % 2 == 0)
            {
                yield return i;
            }
        }
    }

    public static IEnumerable<T> OnlyEvenGeneric<T>(this IEnumerable<T> array)
        where T : INumber<T>
    {
        var two = T.CreateChecked(2);
        var zero = T.Zero;

        foreach (var i in array)
        {
            if (i % two == zero)
            {
                yield return i;
            }
        }
    }
}
