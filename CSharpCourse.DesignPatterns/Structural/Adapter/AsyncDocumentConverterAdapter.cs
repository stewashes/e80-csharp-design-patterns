namespace CSharpCourse.DesignPatterns.Structural.Adapter;

// Async Adapter
// Scenario: Adapting a synchronous API to be asynchronous

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
