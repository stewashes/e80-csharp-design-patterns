namespace CSharpCourse.DesignPatterns.Creational.Factory;

// We use a readonly record struct to represent a point in a 2D plane
// - readonly: the struct is immutable
// - record: provides value equality and a default GetHashCode and ToString implementation
// - struct: the struct is a value type, it is allocated on the stack
internal readonly record struct Point
{
    public double X { get; init; }
    public double Y { get; init; }

    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }
}
