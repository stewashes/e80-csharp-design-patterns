using CSharpCourse.DesignPatterns.Behavioral.ChainOfResponsibility;
using CSharpCourse.DesignPatterns.Tests.Utils;
using Moq;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.ChainOfResponsibilityTests;

public class PackageHandlerTests
{
    [Fact]
    public void RealHouse()
    {
        var package = new Package();

        // Build the chain
        var mailmain = new Mailman();
        var postOffice = new PostOffice(mailmain);
        var warehouse = new Warehouse(postOffice);

        // Call the first ring of the chain
        warehouse.Handle(package);

        // It's easy to change the order of handlers, replace
        // them or add new ones without changing the client code.

        Assert.True(package.IsDelivered);
        Assert.NotEmpty(package.Items);
    }

    [Fact]
    public void FakeHouse()
    {
        var package = new Package();

        var mailman = new Mailman();
        var postOffice = new PostOffice(mailman);

        // We mock the warehouse, but we make sure that it
        // proxies the call to the real warehouse
        var warehouse = new Mock<Warehouse>(postOffice) 
        {
            CallBase = true
        };

        // We add a side effect exclusively to the warehouse's
        // GetAddress method
        warehouse
                .Setup(w => w.GetAddress(It.IsAny<string>()))
                .Returns(new Address
                {
                    Street = "Fictional Street",
                    City = "Nowhere",
                    HouseNumber = "123",
                    IsRealHouse = false
                });

        // With a protected method we would need to call Protected()
        // and then .Setup<Address>("GetAddress", ItExpr.IsAny<string>())

        var output = OutputUtils.CaptureConsoleOutput(
            () => warehouse.Object.Handle(package));

        Assert.False(package.IsDelivered);
        Assert.Empty(package.Items);
        Assert.Contains("Error delivering package: The house does not exist!", output);
    }
}
