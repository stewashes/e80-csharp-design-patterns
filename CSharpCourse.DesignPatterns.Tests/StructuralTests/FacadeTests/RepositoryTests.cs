using CSharpCourse.DesignPatterns.Structural.Facade;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.FacadeTests;

public class RepositoryTests
{
    [Fact]
    public async Task Repository()
    {
        var repository = new CustomerRepository(new AppDbContext());

        var customer = new Customer
        {
            Name = "John Doe",
            Email = "john.doe@example.com"
        };

        var result = await repository.AddAsync(customer);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(customer.Name, result.Name);
        Assert.Equal(customer.Email, result.Email);
        Assert.NotEqual(default, result.CreatedAt);

        // Verify it's in the context
        var savedCustomer = await repository.GetByIdAsync(result.Id);
        Assert.Equal(result.Id, savedCustomer.Id);
    }
}
