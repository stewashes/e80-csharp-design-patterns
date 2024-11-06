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
    public int UndoCount => throw new NotImplementedException();

    public int RedoCount => throw new NotImplementedException();

    public string Content => throw new NotImplementedException();

    public void ChangeContent(string content)
    {
        throw new NotImplementedException();
    }

    public void Redo()
    {
        throw new NotImplementedException();
    }

    public void Undo()
    {
        throw new NotImplementedException();
    }
}
