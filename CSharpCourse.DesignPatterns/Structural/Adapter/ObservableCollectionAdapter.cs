using System.Collections.ObjectModel;

namespace CSharpCourse.DesignPatterns.Structural.Adapter;

// Collection Adapter
// Scenario: Adapting an IEnumerable to work with Observable collections
// (very useful in WPF applications)

internal class ObservableCollectionAdapter<T> : ObservableCollection<T>
{
    public ObservableCollectionAdapter(IEnumerable<T> collection) : base(collection)
    {
    }

    public void AddRange(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Add(item);
        }
    }

    public void RemoveRange(IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            Remove(item);
        }
    }

    public void ReplaceAll(IEnumerable<T> items)
    {
        Clear();
        AddRange(items);
    }
}
