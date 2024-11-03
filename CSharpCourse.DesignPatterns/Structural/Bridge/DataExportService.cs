using System.IO.Compression;
using System.Text.Json;
using System.Text;

namespace CSharpCourse.DesignPatterns.Structural.Bridge;

internal record ExportRequest(
    string Id,
    DateTime Timestamp,
    string DataSetName,
    IEnumerable<Dictionary<string, object>> Data
);

internal record ExportResult(
    string Location,
    long SizeInBytes,
    string ContentType,
    TimeSpan Duration
);

#region First dimension: Data Format
internal interface IDataFormatter
{
    string FileExtension { get; }
    string ContentType { get; }
    byte[] Format(ExportRequest request);
}

internal class JsonFormatter : IDataFormatter
{
    private readonly JsonSerializerOptions _options;

    public JsonFormatter(bool indented = true)
    {
        _options = new JsonSerializerOptions
        {
            WriteIndented = indented,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public string FileExtension => ".json";
    public string ContentType => "application/json";

    public byte[] Format(ExportRequest request)
    {
        var json = JsonSerializer.Serialize(request, _options);
        return Encoding.UTF8.GetBytes(json);
    }
}

internal class CsvFormatter : IDataFormatter
{
    public string FileExtension => ".csv";
    public string ContentType => "text/csv";

    public byte[] Format(ExportRequest request)
    {
        using var ms = new MemoryStream();
        using var writer = new StreamWriter(ms);

        // Write headers
        if (request.Data.Any())
        {
            var headers = request.Data.First().Keys;
            writer.WriteLine(string.Join(",", headers));
        }

        // Write data
        foreach (var row in request.Data)
        {
            var values = row.Values.Select(v => EscapeCsvField(v?.ToString() ?? ""));
            writer.WriteLine(string.Join(",", values));
        }

        writer.Flush();
        return ms.ToArray();
    }

    private static string EscapeCsvField(string field)
    {
        if (field.Contains(',') || field.Contains('"') || field.Contains('\n'))
        {
            return $"\"{field.Replace("\"", "\"\"")}\"";
        }
        return field;
    }
}
#endregion

#region Second dimension: Compression
internal interface ICompressionHandler
{
    string CompressionExtension { get; }
    Task<byte[]> CompressAsync(byte[] data);
    Task<byte[]> DecompressAsync(byte[] compressedData);
}

internal class GZipCompression : ICompressionHandler
{
    public string CompressionExtension => ".gz";

    public async Task<byte[]> CompressAsync(byte[] data)
    {
        using var ms = new MemoryStream();
        using (var gzip = new GZipStream(ms, CompressionLevel.Optimal))
        {
            await gzip.WriteAsync(data);
        }
        return ms.ToArray();
    }

    public async Task<byte[]> DecompressAsync(byte[] compressedData)
    {
        using var input = new MemoryStream(compressedData);
        using var output = new MemoryStream();
        using (var gzip = new GZipStream(input, CompressionMode.Decompress))
        {
            await gzip.CopyToAsync(output);
        }
        return output.ToArray();
    }
}

internal class NoCompression : ICompressionHandler
{
    public string CompressionExtension => "";
    public Task<byte[]> CompressAsync(byte[] data) => Task.FromResult(data);
    public Task<byte[]> DecompressAsync(byte[] compressedData) => Task.FromResult(compressedData);
}
#endregion

#region Third dimension: Storage
internal interface IStorageProvider
{
    Task<ExportResult> StoreAsync(string fileName, byte[] data, string contentType);
    Task<byte[]> RetrieveAsync(string location);
}

internal class LocalStorageProvider : IStorageProvider
{
    private readonly string _basePath;

    public LocalStorageProvider(string basePath)
    {
        _basePath = basePath;
        Directory.CreateDirectory(_basePath);
    }

    public async Task<ExportResult> StoreAsync(string fileName, byte[] data, string contentType)
    {
        var path = Path.Combine(_basePath, fileName);
        var startTime = DateTime.UtcNow;

        await File.WriteAllBytesAsync(path, data);

        return new ExportResult(
            Location: path,
            SizeInBytes: data.Length,
            ContentType: contentType,
            Duration: DateTime.UtcNow - startTime
        );
    }

    public Task<byte[]> RetrieveAsync(string location) =>
        File.ReadAllBytesAsync(location);
}

internal class AzureBlobStorageProvider : IStorageProvider
{
    public AzureBlobStorageProvider(string connectionString, string containerName)
    {
        // ... (initialize Azure Blob Storage client)
    }

    public async Task<ExportResult> StoreAsync(string fileName, byte[] data, string contentType)
    {
        var startTime = DateTime.UtcNow;

        // ... (upload to Azure Blob Storage)
        await Task.Delay(10);

        return new ExportResult(
            Location: "https://example.com",
            SizeInBytes: data.Length,
            ContentType: contentType,
            Duration: DateTime.UtcNow - startTime
        );
    }

    public async Task<byte[]> RetrieveAsync(string location)
    {
        // ... (download from Azure Blob Storage)
        await Task.Delay(10);

        return [];
    }
}
#endregion
