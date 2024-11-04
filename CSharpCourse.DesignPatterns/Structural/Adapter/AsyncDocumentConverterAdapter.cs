namespace CSharpCourse.DesignPatterns.Structural.Adapter;

// Async Adapter
// Scenario: Adapting a synchronous API to be asynchronous

// WARNING: This can be very dangerous! Using Task.Run to wrap a
// synchronous, CPU-bound operation in an asynchronous call is deceiving
// and can lead to thread pool exhaustion and performance issues!

// Read Stephen Toub's article on the subject:
// https://devblogs.microsoft.com/pfxteam/should-i-expose-asynchronous-wrappers-for-synchronous-methods/

internal interface IAsyncDocumentConverter
{
    Task<byte[]> ConvertToPdfAsync(byte[] input);
}

internal class LegacyDocumentConverter
{
    public byte[] ConvertToPdf(byte[] input)
    {
        // CPU-intensive conversion process
        Thread.Sleep(500);
        return [];
    }
}

internal class AsyncDocumentConverterAdapter : IAsyncDocumentConverter
{
    private readonly LegacyDocumentConverter _converter;

    public AsyncDocumentConverterAdapter(LegacyDocumentConverter converter)
    {
        _converter = converter;
    }

    public async Task<byte[]> ConvertToPdfAsync(byte[] input)
    {
        // Using Task.Run is dangerous!
        return await Task.Run(() => _converter.ConvertToPdf(input));
    }
}
