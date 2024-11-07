using CSharpCourse.DesignPatterns.Behavioral.Memento;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.MementoTests;

public class TextEditorTests
{
    [Fact]
    public void TextEditor()
    {
        var editor = new TextEditor();

        editor.Type("Hello ");
        var memento = editor.Save();
        editor.Type("World!");
        editor.Restore(memento);

        Assert.Equal("Hello ", editor.GetContent());
    }
}
