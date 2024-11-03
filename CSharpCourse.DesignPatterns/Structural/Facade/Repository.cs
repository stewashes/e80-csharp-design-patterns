using Microsoft.Extensions.Logging;

namespace CSharpCourse.DesignPatterns.Structural.Facade;

internal class Customer
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public DateTime CreatedAt { get; set; }
}

// This could be a real database context
internal class AppDbContext
{
    private readonly List<Customer> _customers = [];

    public List<Customer> Customers => _customers;
}
