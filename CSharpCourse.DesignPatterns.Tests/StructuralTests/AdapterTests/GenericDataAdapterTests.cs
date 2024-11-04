using CSharpCourse.DesignPatterns.Structural.Adapter;
using Mapster;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.AdapterTests;

public class GenericDataAdapterTests
{
    class CustomerModel
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }

    class FakeCsvDataReader : IDataReader<string[]>
    {
        private readonly string[][] _data;

        public FakeCsvDataReader(string[][] data)
        {
            _data = data;
        }

        public Task<IEnumerable<string[]>> ReadDataAsync()
        {
            return Task.FromResult(_data.AsEnumerable());
        }
    }

    [Fact]
    public async Task GenericAdapter()
    {
        string[][] csvData = [
            ["John Doe", "john@example.com"],
            ["Jane Smith", "jane@example.com"]
        ];

        var csvReader = new FakeCsvDataReader(csvData);
        var adapter = new GenericDataAdapter<string[], CustomerModel>(
            csvReader,
            arr => new CustomerModel { Name = arr[0], Email = arr[1] }
        );

        // Act
        var result = (await adapter.ReadDataAsync()).ToList();

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal("John Doe", result[0].Name);
        Assert.Equal("jane@example.com", result[1].Email);
    }

    [Fact]
    public void AdaptWithMapster()
    {
        var config = new TypeAdapterConfig();
        config.NewConfig<string[], CustomerModel>()
            .Map(dest => dest.Name, src => src[0])
            .Map(dest => dest.Email, src => src[1]);

        string[][] csvData = [
            ["John Doe", "john@example.com"],
            ["Jane Smith", "jane@example.com"]
        ];

        // Be careful with mappers, do not leak business logic into
        // the mapping configuration. Keep it simple and maintainable.
        // Otherwise, it can lead to bugs that are hard to track down.
        var result = csvData.Adapt<CustomerModel[]>(config);

        Assert.Equal(2, result.Length);
        Assert.Equal("John Doe", result[0].Name);
        Assert.Equal("jane@example.com", result[1].Email);
    }

    class CustomerModelDto
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
    }

    [Fact]
    public void AdaptDtoWithMapster()
    {
        // DTO coming from a different component
        var customerDto = new CustomerModelDto { Name = "John Doe", Email = "john@example.com" };

        // Automatic, convention-based mapping (the types differ, but the property names match)
        // If they don't match, you can use a TypeAdapterConfig like in the previous example
        var customer = customerDto.Adapt<CustomerModel>();

        Assert.Equal("John Doe", customer.Name);
        Assert.Equal("john@example.com", customer.Email);
    }
}
