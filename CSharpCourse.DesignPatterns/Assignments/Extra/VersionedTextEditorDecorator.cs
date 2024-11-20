namespace CSharpCourse.DesignPatterns.Assignments.Extra;

internal record TextEditorMemento
{
    public required string Content { get; init; }
}

internal interface ITextEditor
{
    string Content { get; }
    void ChangeContent(string content);
}

// Generic interface for versioned changes
internal interface IVersionedChanges
{
    int UndoCount { get; }
    int RedoCount { get; }
    void Undo();
    void Redo();
}

// Basic text editor without any versioning
internal class TextEditor : ITextEditor
{
    public string Content { get; private set; } = string.Empty;

    public void ChangeContent(string content)
    {
        Content = content;
    }
}

// Base class for versioned changes
internal abstract class VersionedChanges<TMemento> : IVersionedChanges
    where TMemento : class
{
    private readonly Stack<TMemento> _undoStack = new();
    private readonly Stack<TMemento> _redoStack = new();

    public int UndoCount => _undoStack.Count;
    public int RedoCount => _redoStack.Count;

    public void Undo()
    {
        if (_undoStack.Count < 1)
        {
            throw new InvalidOperationException("No more states to undo.");
        }

        var memento = _undoStack.Pop();
        _redoStack.Push(TakeSnapshot());
        UndoInternal(memento);
    }

    protected void SaveCurrentState()
    {
        _undoStack.Push(TakeSnapshot());
        _redoStack.Clear();
    }

    // Template method
    protected abstract void UndoInternal(TMemento memento);

    public void Redo()
    {
        if (_redoStack.Count < 1)
        {
            throw new InvalidOperationException("No more states to redo.");
        }

        var memento = _redoStack.Pop();
        _undoStack.Push(TakeSnapshot());
        RedoInternal(memento);
    }

    // Template method
    protected abstract void RedoInternal(TMemento memento);

    protected abstract TMemento TakeSnapshot();
}

// Decorator for adding versioning to a text editor
internal class VersionedTextEditor : VersionedChanges<TextEditorMemento>, ITextEditor
{
    private readonly ITextEditor _textEditor;

    public VersionedTextEditor(ITextEditor textEditor)
    {
        _textEditor = textEditor;
    }

    public string Content => _textEditor.Content;

    public void ChangeContent(string content)
    {
        SaveCurrentState();
        _textEditor.ChangeContent(content);
    }

    protected override void UndoInternal(TextEditorMemento memento)
        => _textEditor.ChangeContent(memento.Content);

    protected override void RedoInternal(TextEditorMemento memento)
        => _textEditor.ChangeContent(memento.Content);

    protected override TextEditorMemento TakeSnapshot() => new() { Content = _textEditor.Content };
}
