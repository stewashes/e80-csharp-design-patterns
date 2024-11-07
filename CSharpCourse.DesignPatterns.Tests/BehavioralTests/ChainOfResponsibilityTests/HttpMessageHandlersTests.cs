using CSharpCourse.DesignPatterns.Behavioral.ChainOfResponsibility;
using CSharpCourse.DesignPatterns.Tests.Utils;
using Microsoft.Extensions.Caching.Memory;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.ChainOfResponsibilityTests;

public class FirewallMessageHandlerTests
{
    [Fact]
    public async Task Firewall()
    {
        // Check the source code of HttpClientHandler to see
        // how it works.

        // Create the handler chain
        var lastHandler = new HttpClientHandler();
        var firewallHandler = new FirewallMessageHandler(
            ["blocked.example.com"])
        {
            InnerHandler = lastHandler
        };

        using var client = new HttpClient(firewallHandler);

        using var response = await client.GetAsync(
            "https://blocked.example.com/api/data");

        Assert.Equal(System.Net.HttpStatusCode.Forbidden, response.StatusCode);
        var content = await response.Content.ReadAsStringAsync();
        Assert.Contains("blocked by firewall", content);
    }

    [Fact]
    public async Task Chain()
    {
        var memoryCache = new MemoryCache(new MemoryCacheOptions());

        // Create the handler chain
        var lastHandler = new HttpClientHandler();

        var cachingHandler = new CachingHandler(
            memoryCache, TimeSpan.FromMinutes(10))
        {
            InnerHandler = lastHandler
        };

        var loggingHandler = new LoggingHandler()
        {
            InnerHandler = cachingHandler
        };

        var firewallHandler = new FirewallMessageHandler(
            ["blocked.example.com"])
        {
            InnerHandler = loggingHandler
        };

        using var client = new HttpClient(firewallHandler);

        // Make the first request
        var url = "https://example.com";
        using var response1 = await client.GetAsync(url);
        var content1 = await response1.Content.ReadAsStringAsync();

        Assert.Equal(System.Net.HttpStatusCode.OK, response1.StatusCode);
        Assert.Contains("Example Domain", content1);

        // Verify item is in cache
        Assert.Equal(1, memoryCache.Count);

        HttpResponseMessage? response2 = null;

        try
        {
            var output = await OutputUtils.CaptureConsoleOutputAsync(
            async () =>
            {
                response2 = await client.GetAsync(url);
            });

            var content2 = await response2!.Content.ReadAsStringAsync();

            Assert.Equal(System.Net.HttpStatusCode.OK, response2!.StatusCode);
            Assert.Contains("Request: GET https://example.com", output);
            Assert.Contains("Example Domain", content2);
        }
        finally
        {
            response2?.Dispose();
        }
    }
}
