namespace CSharpCourse.DesignPatterns.Behavioral.Iterator;

internal record Product
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Name { get; set; }
    public decimal Price { get; set; }
}
