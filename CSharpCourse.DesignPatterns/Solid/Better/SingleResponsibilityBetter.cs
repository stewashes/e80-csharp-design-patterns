using System.Text.Json;
using System.Text.Json.Serialization;
using CSharpCourse.DesignPatterns.Solid.Enums;

namespace CSharpCourse.DesignPatterns.Solid.Better;

// The only purpose of this record is to hold weather data
internal record WeatherData
{
    public DateTime DateTime { get; init; }
    public double Temperature { get; init; }
    public double Humidity { get; init; }
    public WeatherCondition Condition { get; init; }
    public string? Location { get; init; }
    public double RainProbability { get; init; }
}

// Classes that implement this interface are in charge of validating cities
internal interface ILocationService
{
    Task<bool> IsValidLocationAsync(string location);
}

internal class FakeLocationService : ILocationService
{
    public async Task<bool> IsValidLocationAsync(string location)
    {
        await Task.Delay(100);
        return true;
    }
}

// This is how an actual implementation of the service would look like
internal class RealLocationService : ILocationService, IDisposable
{
    // Reuse a single HttpClient for multiple requests
    private readonly HttpClient _httpClient;
    private bool _disposed = false; // Detect redundant calls
    
    public RealLocationService()
    {
        // In an ASP.NET Core application, this should be injected via
        // dependency injection through the IHttpClientFactory. This way,
        // there is no need to manually dispose of the HttpClient when this
        // class is disposed of.
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("...");
    }
    
    public async Task<bool> IsValidLocationAsync(string location)
    {
        // Validate input parameters
        ArgumentException.ThrowIfNullOrWhiteSpace(location);
        ObjectDisposedException.ThrowIf(_disposed, nameof(RealLocationService));
        
        using var response = await _httpClient.GetAsync($"location/{location}/exists");
        var jsonResponse = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<bool>(jsonResponse);
    }

    // Dispose of the client to prevent memory leaks and socket exhaustion
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        // If already disposed, don't dispose again
        if (_disposed)
        {
            return;
        }
        
        if (disposing)
        {
            // Dispose managed
            _httpClient.Dispose();
        }
            
        // Dispose unmanaged

        _disposed = true;
    }

    ~RealLocationService()
    {
        Dispose(false);
    }
}

// Classes that implement this interface are in charge of saving and
// loading data to and from the persistent storage.
internal interface IStorageService<T> where T : class
{
    Task SaveAsync(T obj, string fileName);
    Task<T?> LoadAsync(string fileName);
}

internal class LocalStorageService<T> : IStorageService<T> where T : class
{
    private readonly JsonSerializerOptions _jsonOptions = new();
    
    public LocalStorageService()
    {
        _jsonOptions.Converters.Add(new JsonStringEnumConverter());
        _jsonOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        _jsonOptions.WriteIndented = true;
    }
    
    public async Task SaveAsync(T obj, string fileName)
    {
        await File.WriteAllTextAsync(fileName, JsonSerializer.Serialize(obj, _jsonOptions));
    }

    public async Task<T?> LoadAsync(string fileName)
    {
        var json = await File.ReadAllTextAsync(fileName);
        return JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }
}
