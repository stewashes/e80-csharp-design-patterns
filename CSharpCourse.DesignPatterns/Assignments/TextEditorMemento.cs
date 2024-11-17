namespace CSharpCourse.DesignPatterns.Assignments;

internal record TextEditorMemento
{
    public required string Content { get; init; }
}

internal interface IVersionedTextEditor
{
    string Content { get; }
    int UndoCount { get; }
    int RedoCount { get; }
    void ChangeContent(string content);
    void Undo();
    void Redo();
}

internal class VersionedTextEditor : IVersionedTextEditor
{
    private readonly Stack<TextEditorMemento> _undoHistory = new();
    private readonly Stack<TextEditorMemento> _redoHistory = new();
    private string _content = string.Empty;
    
    public int UndoCount => _undoHistory.Count;

    public int RedoCount => _redoHistory.Count;
    
    public string Content => _content;

    public void ChangeContent(string content)
    {
        if (content == _content) 
            return;

        var snapshot = new TextEditorMemento { Content = _content };
        
        _undoHistory.Push(snapshot);
        _content = content;
        _redoHistory.Clear();
    }

    public void Redo()
    {
        if (_redoHistory.Count == 0)
            throw new InvalidOperationException("Cannot redo");

        var snapshot = new TextEditorMemento { Content = _content };

        _undoHistory.Push(snapshot);
        _content = _redoHistory.Pop().Content;
    }

    public void Undo()
    {
        if (_undoHistory.Count == 0)
            throw new InvalidOperationException("Cannot undo");

        var snapshot = new TextEditorMemento { Content = _content };

        _redoHistory.Push(snapshot);
        _content = _undoHistory.Pop().Content;
    }
}