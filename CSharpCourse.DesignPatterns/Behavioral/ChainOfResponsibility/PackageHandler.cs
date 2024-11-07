namespace CSharpCourse.DesignPatterns.Behavioral.ChainOfResponsibility;

internal record Item(string Name, double Price, double Weight);

internal class Package
{
    public string OrderId { get; set; } = Guid.NewGuid().ToString();
    public List<Item> Items { get; } = [];
    public Address? Address { get; set; }
    public double PostFees { get; set; }
    public bool IsDelivered { get; set; }

    public void Deliver()
    {
        if (!Address!.IsRealHouse)
        {
            throw new InvalidAddressException("The house does not exist!");
        }

        IsDelivered = true;
        Console.WriteLine($"Package with {Items.Count} items delivered!");
    }
}

internal class Address
{
    public required string Street { get; set; }
    public required string City { get; set; }
    public required string HouseNumber { get; set; }
    public bool IsRealHouse { get; set; } = true;
}

public class InvalidAddressException : Exception
{
    public InvalidAddressException(string message) : base(message) { }
}

internal abstract class PackageHandler
{
    protected PackageHandler? Next;

    protected PackageHandler(PackageHandler? next = null)
    {
        Next = next;
    }

    public virtual void Handle(Package package) => Next?.Handle(package);
}

internal class Warehouse : PackageHandler
{
    public Warehouse(PackageHandler? next = null) : base(next) { }

    public override void Handle(Package package)
    {
        // Put the items in the package
        var items = GetOrderItems(package.OrderId);
        package.Items.AddRange(items);

        // Stick the address label on the package
        package.Address = GetAddress(package.OrderId);

        try
        {
            base.Handle(package);
        }
        catch (InvalidAddressException ex)
        {
            Console.WriteLine($"Error delivering package: {ex.Message}");

            // Take out the items from the package
            package.Items.Clear();

            // Remove the address label
            package.Address = null;
        }
    }

    private static IEnumerable<Item> GetOrderItems(string orderId)
    {
        Console.WriteLine($"Fetching items for order {orderId}...");

        return
        [
            new("Electric Toothbrush", Price: 29.99, Weight: .05),
            new("Roomba", Price: 999.99, Weight: 4)
        ];
    }

    // We declare this as internal and virtual in order to be able to
    // easily mock it. We could also do it for protected methods using
    // Moq's Protected() method.
    // We could even mock private methods but we would need to use
    // reflection and it's generally not recommended.
    internal virtual Address GetAddress(string orderId)
    {
        Console.WriteLine($"Fetching address for order {orderId}...");

        return new()
        {
            Street = "Evergreen Terrace",
            City = "Springfield",
            HouseNumber = "742",
            IsRealHouse = true
        };
    }
}

internal class PostOffice : PackageHandler
{
    public PostOffice(PackageHandler? next = null) : base(next) { }

    public override void Handle(Package package)
    {
        package.PostFees = CalcFee(package);

        try
        {
            base.Handle(package);
        }
        catch (InvalidAddressException)
        {
            package.PostFees = 0;
            throw;
        }
    }

    private static double CalcFee(Package package)
    {
        var totalWeight = package.Items.Sum(i => i.Weight);

        // 1€ for each kg
        return Math.Round(totalWeight, 2);
    }
}

internal class Mailman : PackageHandler
{
    public Mailman(PackageHandler? next = null) : base(next) { }

    public override void Handle(Package package)
    {
        package.Deliver();

        // If there's no more handler configured here (e.g., the
        // delivery is to a real house and not to a PO box), the
        // Next property will be null, and the base method will
        // end the chain.
        //
        // Otherwise, if we wanted to forcibly
        // break out of the chain, we could just not call the
        // base method.
        base.Handle(package);
    }
}
