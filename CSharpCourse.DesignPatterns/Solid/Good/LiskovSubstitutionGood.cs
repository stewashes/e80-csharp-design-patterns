namespace CSharpCourse.DesignPatterns.Solid.Good;

internal interface IShape
{
    int CalculateArea();
}

internal readonly record struct Rectangle : IShape
{
    public int Width { get; init; }
    public int Height { get; init; }

    public int CalculateArea()
    {
        return Width * Height;
    }
}

internal readonly record struct Square : IShape
{
    public int Side { get; init; }

    public int CalculateArea()
    {
        return Side * Side;
    }
}
