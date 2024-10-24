using CSharpCourse.DesignPatterns.Creational.Factory;

namespace CSharpCourse.DesignPatterns.Tests.CreationalTests.FactoryTests;

public class PointFactoryTests
{
    [Fact]
    public void CartesianMethod()
    {
        var point = Point.NewCartesianPoint(3, 4);
        Assert.Equal(3, point.X);
        Assert.Equal(4, point.Y);
    }

    [Fact]
    public void PolarMethod()
    {
        var point = Point.NewPolarPoint(1, Math.PI / 2);
        Assert.Equal(0, point.X, 3);
        Assert.Equal(1, point.Y, 3);
    }

    [Fact]
    public void OriginProperty()
    {
        var origin = Point.Origin;
        Assert.Equal(0, origin.X);
        Assert.Equal(0, origin.Y);
    }

    [Fact]
    public void PointFactoryCartesian()
    {
        var point = PointFactory.NewCartesianPoint(3, 4);
        Assert.Equal(3, point.X);
        Assert.Equal(4, point.Y);
    }

    [Fact]
    public void PointFactoryPolar()
    {
        var point = PointFactory.NewPolarPoint(1, Math.PI / 2);
        Assert.Equal(0, point.X, 3);
        Assert.Equal(1, point.Y, 3);
    }

    [Fact]
    public void InnerPointFactoryCartesian()
    {
        var point = Point.Factory.NewCartesianPoint(3, 4);
        Assert.Equal(3, point.X);
        Assert.Equal(4, point.Y);
    }

    [Fact]
    public void InnerPointFactoryPolar()
    {
        var point = Point.Factory.NewPolarPoint(1, Math.PI / 2);
        Assert.Equal(0, point.X, 3);
        Assert.Equal(1, point.Y, 3);
    }
}
