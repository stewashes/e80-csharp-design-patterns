using CSharpCourse.DesignPatterns.Creational.Builder;

namespace CSharpCourse.DesignPatterns.Tests.CreationalTests.BuilderTests;

public class PersonBuilderTests
{
    [Fact]
    public void FacetedBuilder()
    {
        var person = PersonBuilder
            .CreatePerson()
            .Lives
                .At("Via Manzoni 1")
                .In("Milano")
                .WithPostalCode("20121")
            .Works
                .At("Big Corp Inc.")
                .AsA("Software Developer")
                .Earning(45_000)
            .Build();
        
        Assert.Equal("Via Manzoni 1", person.Address.Street);
        Assert.Equal("Milano", person.Address.City);
        Assert.Equal("20121", person.Address.PostalCode);
        
        Assert.Equal("Big Corp Inc.", person.Job.Company);
        Assert.Equal("Software Developer", person.Job.Title);
        Assert.Equal(45_000, person.Job.Salary);
    }
}
