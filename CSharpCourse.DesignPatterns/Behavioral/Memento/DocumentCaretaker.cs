namespace CSharpCourse.DesignPatterns.Behavioral.Memento;

internal class Document
{
    public string Content { get; private set; } = string.Empty;
    public string Author { get; private set; } = string.Empty;

    public void UpdateContent(string content, string author)
    {
        Content = content;
        Author = author;
    }

    public DocumentMemento CreateMemento() => new(Content, Author);
    public void RestoreFromMemento(DocumentMemento memento)
    {
        Content = memento.Content;
        Author = memento.Author;
    }
}

internal record DocumentMemento
{
    public string Content { get; }
    public string Author { get; }
    public DateTime Timestamp { get; }

    public DocumentMemento(string content, string author)
    {
        Content = content;
        Author = author;
        Timestamp = DateTime.UtcNow;
    }
}

// The Caretaker is the class that holds onto (Takes care of) the collection
// of Memento classes and decides when to checkpoint or roll back the data.
internal class DocumentCaretaker
{
    private readonly Stack<DocumentMemento> _history = new();

    public void SaveVersion(Document document)
    {
        _history.Push(document.CreateMemento());
    }

    public void RestoreLastVersion(Document document)
    {
        if (_history.Count > 0)
        {
            var memento = _history.Pop();
            document.RestoreFromMemento(memento);
        }
    }

    public int GetVersionCount() => _history.Count;
}
