using CSharpCourse.DesignPatterns.Structural.Bridge;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.BridgeTests;

public class DataExportServiceTests
{
    [Fact]
    public async Task ExportService()
    {
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        var service = new DataExportService(
            new JsonFormatter(),
            new GZipCompression(),
            new LocalStorageProvider(tempPath)
        );

        var request = new ExportRequest(
            Id: "TEST-001",
            Timestamp: DateTime.UtcNow,
            DataSetName: "TestExport",
            Data:
            [
                new Dictionary<string, object>
                {
                    ["id"] = 1,
                    ["name"] = "Test Item",
                    ["value"] = 42.5
                }
            ]
        );

        try
        {
            var result = await service.ExportAsync(request);

            Assert.NotNull(result);
            Assert.True(result.SizeInBytes > 0);
            Assert.True(File.Exists(result.Location));
            Assert.EndsWith(".json.gz", result.Location);
            Assert.Equal("application/json", result.ContentType);
        }
        finally
        {
            // Cleanup
            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }
        }
    }

    [Fact]
    public async Task ExportServiceDependencyInjection()
    {
        var services = new ServiceCollection();
        var tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

        // Register all implementations with appropriate scopes
        services.AddScoped<IDataFormatter, JsonFormatter>();
        services.AddScoped<ICompressionHandler, GZipCompression>();
        services.AddScoped<IStorageProvider>(sp => new LocalStorageProvider(tempPath));

        // Register the main service
        services.AddScoped<DataExportService>();

        // Build the service provider
        var serviceProvider = services.BuildServiceProvider();

        // Create a scope for our dependencies. The same instance of DataExportService
        // and its dependencies will be retrieved within this scope each time it's requested.
        // This is done to avoid creating too many instances of the same service
        // if we need to export multiple datasets, which is very helpful for example when
        // we want to reuse the same connection to a server.
        using var scope = serviceProvider.CreateScope();
        var exportService = scope.ServiceProvider.GetRequiredService<DataExportService>();

        var request = new ExportRequest(
            Id: "TEST-002",
            Timestamp: DateTime.UtcNow,
            DataSetName: "TestExportDI",
            Data:
            [
                new Dictionary<string, object>
                {
                    ["id"] = 1,
                    ["name"] = "DI Test Item",
                    ["value"] = 42.5
                }
            ]
        );

        try
        {
            var result = await exportService.ExportAsync(request);

            Assert.NotNull(result);
            Assert.True(result.SizeInBytes > 0);
            Assert.True(File.Exists(result.Location));
            Assert.EndsWith(".json.gz", result.Location);
            Assert.Equal("application/json", result.ContentType);
        }
        finally
        {
            // Cleanup
            if (!string.IsNullOrEmpty(tempPath) && Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
            }
        }
    }
}
