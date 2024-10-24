namespace CSharpCourse.DesignPatterns.Creational.Factory;

// We use a readonly record struct to represent a point in a 2D plane
// - readonly: the struct is immutable
// - record: provides value equality and a default GetHashCode and ToString implementation
// - struct: the struct is a value type, it is allocated on the stack
internal readonly record struct Point
{
    public double X { get; init; }
    public double Y { get; init; }

    // We could make this constructor private to force the use
    // of factory methods
    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    #region Factory methods
    public static Point NewCartesianPoint(double x, double y)
    {
        return new Point(x, y);
    }

    public static Point NewPolarPoint(double rho, double theta)
    {
        return new Point(
            rho * Math.Cos(theta),
            rho * Math.Sin(theta));
    }
    #endregion

    #region Factory properties
    public static Point Origin => new(0, 0);
    #endregion

    #region Inner Factory
    // If we want to keep the constructor private, we can use an inner factory
    // class to create points.
    // An example of this in the wild is Task.Factory.StartNew in the TPL
    internal static class Factory
    {
        public static Point NewCartesianPoint(double x, double y)
        {
            return new Point(x, y);
        }

        public static Point NewPolarPoint(double rho, double theta)
        {
            return new Point(
                rho * Math.Cos(theta),
                rho * Math.Sin(theta));
        }
    }
    #endregion
}

#region Classic Factory
// Single Responsibility Principle: a class should have only one reason to change
// We use a separate factory class to create points
internal static class PointFactory
{
    // For complex construction, this method can also take
    // a Settings object to configure the object instead of
    // passing many parameters
    public static Point NewCartesianPoint(double x, double y)
    {
        // We need Point's constructor to be public, since it is
        // a separate class
        return new Point(x, y);
    }

    public static Point NewPolarPoint(double rho, double theta)
    {
        return new Point(
            rho * Math.Cos(theta),
            rho * Math.Sin(theta));
    }
}
#endregion
