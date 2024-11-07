namespace CSharpCourse.DesignPatterns.Behavioral.Memento;

internal class TextEditor
{
    private string _content = string.Empty;

    public void Type(string text) => _content += text;
    public string GetContent() => _content;

    public EditorMemento Save() => new(_content);
    public void Restore(EditorMemento memento) => _content = memento.GetSavedContent();
}

internal record EditorMemento
{
    private readonly string _content;
    public EditorMemento(string content) => _content = content;
    public string GetSavedContent() => _content;
}
