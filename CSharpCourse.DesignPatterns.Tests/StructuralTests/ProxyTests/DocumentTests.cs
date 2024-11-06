using CSharpCourse.DesignPatterns.Structural.Proxy;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.ProxyTests;

public class DocumentTests
{
    [Fact]
    public void DocumentProxy()
    {
        var proxy = new DocumentProxy("document.txt");

        Assert.Null(proxy.Document);

        proxy.DisplayContent();

        Assert.NotNull(proxy.Document);
    }
}
