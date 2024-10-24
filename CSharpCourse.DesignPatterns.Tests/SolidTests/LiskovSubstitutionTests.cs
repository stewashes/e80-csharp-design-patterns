namespace CSharpCourse.DesignPatterns.Tests.SolidTests;

public class LiskovSubstitutionTests
{
    // This method doesn't know (and doesn't care) if the
    // rectangle variable is actually a Square, since it expects
    // derived classes to behave the same as their parent
    private static bool TestRectangle(Solid.Bad.Rectangle rectangle)
    {
        rectangle.Width = 5;
        rectangle.Height = 10;
        return rectangle.CalculateArea() == 50;
    }
    
    private static bool TestShape(Solid.Good.IShape shape, int expectedArea)
    {
        return shape.CalculateArea() == expectedArea;
    }
    
    [Fact]
    public void Bad()
    {
        var rect = new Solid.Bad.Rectangle();
        Assert.True(TestRectangle(rect));

        Solid.Bad.Rectangle square = new Solid.Bad.Square();
        Assert.False(TestRectangle(square)); // LSP Violation
    }

    [Fact]
    public void Good()
    {
        var rect = new Solid.Good.Rectangle
        {
            Width = 5,
            Height = 10
        };
        Assert.True(TestShape(rect, 50));

        var square = new Solid.Good.Square
        {
            Side = 5
        };
        Assert.True(TestShape(square, 25));
    }
}
