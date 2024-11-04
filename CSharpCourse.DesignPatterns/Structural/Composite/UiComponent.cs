namespace CSharpCourse.DesignPatterns.Structural.Composite;

// Real-world application of the composite pattern.
// Other examples include file systems, drawing applications, etc.

internal interface IUiComponent
{
    void Render();
    string Name { get; }
}

internal class Button : IUiComponent
{
    public string Name { get; }

    public Button(string name)
    {
        Name = name;
    }

    public void Render()
    {
        Console.WriteLine($"Rendering button: {Name}");
    }
}

internal class Panel : IUiComponent
{
    private readonly List<IUiComponent> components = [];
    public string Name { get; }

    public Panel(string name)
    {
        Name = name;
    }

    public void Add(IUiComponent component) => components.Add(component);

    // When we render a panel, we also render all the components it contains.
    public void Render()
    {
        Console.WriteLine($"Rendering panel: {Name}");
        foreach (var component in components)
        {
            component.Render();
        }
    }
}
