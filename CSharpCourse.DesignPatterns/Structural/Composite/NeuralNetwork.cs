using System.Collections;
using System.Collections.ObjectModel;

namespace CSharpCourse.DesignPatterns.Structural.Composite;

internal class Neuron
{
    public float Value { get; set; }
    public IList<Neuron> In { get; } = [];
    public IList<Neuron> Out { get; } = [];

    public Neuron(float value)
    {
        Value = value;
    }
}

internal class NeuronLayer : Collection<Neuron>
{

}
