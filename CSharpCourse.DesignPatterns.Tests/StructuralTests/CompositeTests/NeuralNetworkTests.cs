using CSharpCourse.DesignPatterns.Structural.Composite;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.CompositeTests;

public class NeuralNetworkTests
{
    [Fact]
    public void NeuronToNeuron()
    {
        var neuron1 = new Neuron(1);
        var neuron2 = new Neuron(2);

        neuron1.ConnectTo(neuron2);

        Assert.Single(neuron1.Out);
        Assert.Single(neuron2.In);
        Assert.Same(neuron2, neuron1.Out[0]);
        Assert.Same(neuron1, neuron2.In[0]);
    }

    [Fact]
    public void NeuronToNeuronLayer()
    {
        var neuron1 = new Neuron(1);
        var neuron2 = new Neuron(2);
        var neuron3 = new Neuron(3);
        var neuronLayer = new NeuronLayer { neuron2, neuron3 };

        neuron1.ConnectTo(neuronLayer);

        Assert.Equal(2, neuron1.Out.Count);
        Assert.Single(neuron2.In);
        Assert.Single(neuron3.In);
        Assert.Same(neuron2, neuron1.Out[0]);
        Assert.Same(neuron3, neuron1.Out[1]);
        Assert.Same(neuron1, neuron2.In[0]);
        Assert.Same(neuron1, neuron3.In[0]);
    }

    [Fact]
    public void NeuronLayerToNeuron()
    {
        var neuron1 = new Neuron(1);
        var neuron2 = new Neuron(2);
        var neuron3 = new Neuron(3);
        var neuronLayer = new NeuronLayer { neuron1, neuron2 };

        neuronLayer.ConnectTo(neuron3);

        Assert.Single(neuron1.Out);
        Assert.Single(neuron2.Out);
        Assert.Equal(2, neuron3.In.Count);
        Assert.Same(neuron3, neuron1.Out[0]);
        Assert.Same(neuron3, neuron2.Out[0]);
        Assert.Same(neuron1, neuron3.In[0]);
        Assert.Same(neuron2, neuron3.In[1]);
    }

    [Fact]
    public void NeuronLayerToNeuronLayer()
    {
        var neuron1 = new Neuron(1);
        var neuron2 = new Neuron(2);
        var neuron3 = new Neuron(3);
        var neuron4 = new Neuron(4);
        var neuronLayer1 = new NeuronLayer { neuron1, neuron2 };
        var neuronLayer2 = new NeuronLayer { neuron3, neuron4 };

        neuronLayer1.ConnectTo(neuronLayer2);

        Assert.Equal(2, neuron1.Out.Count);
        Assert.Equal(2, neuron2.Out.Count);
        Assert.Equal(2, neuron3.In.Count);
        Assert.Equal(2, neuron4.In.Count);
        Assert.Same(neuron3, neuron1.Out[0]);
        Assert.Same(neuron4, neuron1.Out[1]);
        Assert.Same(neuron3, neuron2.Out[0]);
        Assert.Same(neuron4, neuron2.Out[1]);
        Assert.Same(neuron1, neuron3.In[0]);
        Assert.Same(neuron2, neuron3.In[1]);
        Assert.Same(neuron1, neuron4.In[0]);
        Assert.Same(neuron2, neuron4.In[1]);
    }
}
