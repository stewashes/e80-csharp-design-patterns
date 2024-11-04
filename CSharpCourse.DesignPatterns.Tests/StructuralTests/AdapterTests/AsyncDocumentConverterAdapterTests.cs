using CSharpCourse.DesignPatterns.Structural.Adapter;
using System.Diagnostics;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.AdapterTests;

public class AsyncDocumentConverterAdapterTests
{
    [Fact]
    public async Task AsyncAdapter()
    {
        var input = new byte[] { 1, 2, 3, 4, 5 };
        var legacyConverter = new LegacyDocumentConverter();
        var asyncConverter = new AsyncDocumentConverterAdapter(legacyConverter);

        var stopwatch = Stopwatch.StartNew();
        var resultTask = asyncConverter.ConvertToPdfAsync(input);

        Assert.True(stopwatch.ElapsedMilliseconds < 100);

        await resultTask;
    }
}
