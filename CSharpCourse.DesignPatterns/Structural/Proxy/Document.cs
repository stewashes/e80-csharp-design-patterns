namespace CSharpCourse.DesignPatterns.Structural.Proxy;

internal interface IDocument
{
    void DisplayContent();
}

internal class Document : IDocument
{
    public Document(string filename)
    {
        // Simulate loading a large document. This could be improved
        // with async calls, but imagine this is a legacy or third-party class
        // that cannot be changed.
        Console.WriteLine("Loading document from " + filename);
        Content = "Document Content";
    }

    public string Content { get; private set; }

    public void DisplayContent()
    {
        Console.WriteLine(Content);
    }
}
