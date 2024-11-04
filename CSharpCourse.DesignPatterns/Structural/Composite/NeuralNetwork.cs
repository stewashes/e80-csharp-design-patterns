using System.Collections;
using System.Collections.ObjectModel;

namespace CSharpCourse.DesignPatterns.Structural.Composite;

// Without the composite pattern, we would have to write a lot of boilerplate code to connect:
// - Neurons to Neurons
// - Neurons to NeuronLayers
// - NeuronLayers to Neurons
// - NeuronLayers to NeuronLayers
// ... and more if we add collections in the future (e.g. sub-network, neuron ring, etc.)

// We can make the Neuron enumerable (iterable) itself
internal class Neuron : IEnumerable<Neuron>
{
    public float Value { get; set; }
    public IList<Neuron> In { get; } = [];
    public IList<Neuron> Out { get; } = [];

    public Neuron(float value)
    {
        Value = value;
    }

    // When enumerated, it will return itself as the only element
    public IEnumerator<Neuron> GetEnumerator()
    {
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

internal class NeuronLayer : Collection<Neuron> // A collection already implements IEnumerable
{

}

// We write an extension method (or it could just be a normal method as well)
internal static class ExtensionMethods
{
    public static void ConnectTo(this IEnumerable<Neuron> self, IEnumerable<Neuron> other)
    {
        // Do not connect to yourself
        if (ReferenceEquals(self, other)) return;

        // Connect each neuron in the left collection to each neuron in the right collection
        foreach (var from in self)
        {
            foreach (var to in other)
            {
                from.Out.Add(to);
                to.In.Add(from);
            }
        }
    }
}
