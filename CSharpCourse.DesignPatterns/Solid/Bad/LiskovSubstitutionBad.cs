namespace CSharpCourse.DesignPatterns.Solid.Bad;

internal record Rectangle
{
    public virtual int Width { get; set; }
    public virtual int Height { get; set; }

    public int CalculateArea() => Width * Height;
}

internal record Square : Rectangle
{
    // Modifies the behavior of the parent class
    // (something similar could be achieved by using the 'new' keyword)
    public override int Width
    {
        set => base.Width = base.Height = value;
    }

    // Modifies the behavior of the parent class
    // (something similar could be achieved by using the 'new' keyword)
    public override int Height
    {
        set => base.Width = base.Height = value;
    }
}
