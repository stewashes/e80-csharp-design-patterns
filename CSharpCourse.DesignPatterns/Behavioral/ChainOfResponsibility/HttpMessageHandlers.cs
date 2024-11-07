using Microsoft.Extensions.Caching.Memory;

namespace CSharpCourse.DesignPatterns.Behavioral.ChainOfResponsibility;

internal class FirewallMessageHandler : DelegatingHandler
{
    private readonly HashSet<string> _blockedHosts;

    public FirewallMessageHandler(HashSet<string> blockedHosts)
    {
        _blockedHosts = blockedHosts;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var host = request.RequestUri?.Host.ToLowerInvariant();

        if (string.IsNullOrEmpty(host))
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest)
            {
                Content = new StringContent("Invalid request URI")
            };
        }

        if (_blockedHosts.Contains(host))
        {
            return new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
            {
                Content = new StringContent($"Access to {host} is blocked by firewall")
            };
        }

        // If host is not blocked, continue down the handler chain
        return await base.SendAsync(request, cancellationToken);
    }
}

internal class LoggingHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Request: {request.Method} {request.RequestUri}");

        var response = await base.SendAsync(request, cancellationToken);

        Console.WriteLine($"[{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss}] Response: {(int)response.StatusCode} {response.StatusCode}");

        return response;
    }
}

internal class CachingHandler : DelegatingHandler
{
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheDuration;

    public CachingHandler(IMemoryCache cache, TimeSpan cacheDuration)
    {
        _cache = cache;
        _cacheDuration = cacheDuration;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Only cache GET requests
        if (request.Method != HttpMethod.Get)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var key = request.RequestUri?.ToString();
        if (string.IsNullOrEmpty(key))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        // Check cache
        if (_cache.TryGetValue(key, out HttpResponseMessage? cachedResponse))
        {
            return await CloneResponseAsync(cachedResponse!);
        }

        // Cache miss - get fresh response
        var response = await base.SendAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            // We need to clone it, otherwise the stream
            // will be disposed and we can't read it again
            var clone = await CloneResponseAsync(response);
            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(_cacheDuration);

            _cache.Set(key, clone, cacheOptions);
        }

        return response;
    }

    private static async Task<HttpResponseMessage> CloneResponseAsync(
        HttpResponseMessage response)
    {
        var newResponse = new HttpResponseMessage(response.StatusCode);
        
        if (response.Content is not null)
        {
            var content = await response.Content.ReadAsStringAsync();
            newResponse.Content = new StringContent(content);
        }

        // In reality we would also set all other properties like
        // RequestMessage, Headers, etc.

        return newResponse;
    }
}
