namespace CSharpCourse.DesignPatterns.Behavioral.ChainOfResponsibility;

internal record Item(string Name, double Price, double Weight);

internal class Package
{
    public string OrderId { get; set; } = Guid.NewGuid().ToString();
    public List<Item> Items { get; } = [];
    public Address? Address { get; set; }
    public double PostFees { get; set; }
    public bool IsDelivered { get; set; }
}

internal class Address
{
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string HouseNumber { get; set; }
    public bool IsRealHouse { get; set; } = true;
}
