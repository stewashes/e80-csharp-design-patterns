namespace CSharpCourse.DesignPatterns.Structural.Adapter;

// Generic Adapter
// Scenario: Adapting different types of data sources to a common interface

internal interface IDataReader<T>
{
    Task<IEnumerable<T>> ReadDataAsync();
}
