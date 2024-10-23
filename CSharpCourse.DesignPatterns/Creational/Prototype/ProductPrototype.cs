namespace CSharpCourse.DesignPatterns.Creational.Prototype;

internal record Product
{
    public string Name { get; set; }
    public string Brand { get; set; }
    public Size Size { get; set; }

    public Product(string name, string brand, Size size)
    {
        Name = name;
        Brand = brand;
        Size = size;
    }
}

internal record Size
{
    public double Width { get; set; }
    public double Height { get; set; }
    public double Depth { get; set; }

    public Size(double width, double height, double depth)
    {
        Width = width;
        Height = height;
        Depth = depth;
    }
}
