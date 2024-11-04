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

// Proxy class (lazy-loaded)
internal class DocumentProxy : IDocument
{
    private Document? _document;
    private readonly string _filename;

    public Document? Document => _document;

    public DocumentProxy(string filename)
    {
        _filename = filename;
    }

    public void DisplayContent()
    {
        _document ??= new Document(_filename); // Lazy loading
        _document.DisplayContent();
    }
}

// Similarly, a proxy can also be used to
// - add logging
// - add caching
// - mock
// etc...
