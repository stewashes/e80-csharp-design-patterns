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

internal class PersonBuilder
{
    protected readonly Person Person;

    protected PersonBuilder(Person? person = null)
    {
        Person = person ?? new Person();
    }

    public static PersonBuilder CreatePerson()
        => new();
    
    public PersonJobBuilder Works => new PersonJobBuilder(Person);
    public PersonAddressBuilder Lives => new PersonAddressBuilder(Person);
    public Person Build() => Person;
}

internal class PersonJobBuilder : PersonBuilder
{
    public PersonJobBuilder(Person person) : base(person) { }

    public PersonJobBuilder At(string company)
    {
        Person.Job.Company = company;
        return this;
    }

    public PersonJobBuilder AsA(string title)
    {
        Person.Job.Title = title;
        return this;
    }

    public PersonJobBuilder Earning(BigInteger salary)
    {
        Person.Job.Salary = salary;
        return this;
    }
}

internal class PersonAddressBuilder : PersonBuilder
{
    public PersonAddressBuilder(Person person) : base(person) { }

    public PersonAddressBuilder At(string address)
    {
        Person.Address.Street = address;
        return this;
    }

    public PersonAddressBuilder In(string city)
    {
        Person.Address.City = city;
        return this;
    }

    public PersonAddressBuilder WithPostalCode(string postalCode)
    {
        Person.Address.PostalCode = postalCode;
        return this;
    }
}
