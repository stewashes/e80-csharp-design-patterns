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

// The repository is in charge of managing the Customer entity
// by providing an interface to interact with the database
// in a transparent way.
internal interface ICustomerRepository
{
    Task<Customer> GetByIdAsync(Guid id);
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer> AddAsync(Customer customer);
    Task UpdateAsync(Customer customer);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Customer>> FindByEmailAsync(string email);
}

// The repository can also perform validation, logging, caching, etc.
// (or we could introduce decorators for these purposes)
internal class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _context;

    public CustomerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> GetByIdAsync(Guid id)
    {
        // Simulate async operation
        await Task.Delay(10);

        return _context.Customers.Find(c => c.Id == id)
            ?? throw new KeyNotFoundException($"Customer with ID {id} not found");
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        // Simulate async operation
        await Task.Delay(10);

        return [.. _context.Customers];
    }

    public async Task<Customer> AddAsync(Customer customer)
    {
        if (string.IsNullOrWhiteSpace(customer.Email))
        {
            throw new ArgumentException("Email is required");
        }

        if (_context.Customers.Exists(c => c.Email == customer.Email))
        {
            throw new InvalidOperationException("Customer with this email already exists");
        }

        customer.CreatedAt = DateTime.UtcNow;
        customer.Id = Guid.NewGuid();

        // Simulate async operation
        await Task.Delay(10);

        _context.Customers.Add(customer);

        return customer;
    }

    public async Task UpdateAsync(Customer customer)
    {
        var existing = await GetByIdAsync(customer.Id);

        // In a real application, you might use an object mapper here
        existing.Name = customer.Name;
        existing.Email = customer.Email;
    }

    public async Task DeleteAsync(Guid id)
    {
        var customer = await GetByIdAsync(id);
        _context.Customers.Remove(customer);
    }

    public async Task<IEnumerable<Customer>> FindByEmailAsync(string email)
    {
        // Simulate async operation
        await Task.Delay(10);

        return _context.Customers
            .Where(c => c.Email.Contains(email, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }
}
