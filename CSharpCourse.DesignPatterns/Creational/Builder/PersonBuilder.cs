using System.Numerics;

namespace CSharpCourse.DesignPatterns.Creational.Builder;

internal record Person
{
    public PersonJob Job { get; } = new();
    public PersonAddress Address { get; } = new();
}

internal record PersonJob
{
    public string? Company { get; set; }
    public string? Title { get; set; }
    public BigInteger Salary { get; set; }
}

internal record PersonAddress
{
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? PostalCode { get; set; }
}
