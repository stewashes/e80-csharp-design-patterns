namespace CSharpCourse.DesignPatterns.Structural.Adapter;

// Generic Adapter
// Scenario: Adapting different types of data sources to a common interface

internal interface IDataReader<T>
{
    Task<IEnumerable<T>> ReadDataAsync();
}

internal class GenericDataAdapter<TSource, TTarget> : IDataReader<TTarget>
{
    private readonly Func<TSource, TTarget> _converter;
    private readonly IDataReader<TSource> _sourceReader;

    public GenericDataAdapter(IDataReader<TSource> sourceReader, Func<TSource, TTarget> converter)
    {
        _sourceReader = sourceReader;
        _converter = converter;
    }

    public async Task<IEnumerable<TTarget>> ReadDataAsync()
    {
        var sourceData = await _sourceReader.ReadDataAsync();
        return sourceData.Select(_converter);
    }
}
