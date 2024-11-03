using CSharpCourse.DesignPatterns.Structural.Facade;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.FacadeTests;

public class HttpClientFacadeTests
{
    [Fact]
    public async Task Complex()
    {
        using var httpClient = new MyHttpClient();

        // The user of this class has to:
        // 1. Get the IP address of the domain
        // 2. Connect to the server
        // 3. Send a GET request
        var ip = MyHttpClient.GetIpAddress("example.com");
        await httpClient.ConnectAsync(ip, 443, ssl: true);
        var response = await httpClient.SendGetRequestAsync("example.com", "/");

        Assert.Contains("200 OK", response);
        Assert.Contains("Example Domain", response);
    }
}
