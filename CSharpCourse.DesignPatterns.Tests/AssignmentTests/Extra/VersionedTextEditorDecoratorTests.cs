using CSharpCourse.DesignPatterns.Assignments.Extra;

namespace CSharpCourse.DesignPatterns.Tests.AssignmentTests.Extra;

public class VersionedTextEditorDecoratorTests
{
    [Fact]
    public void VersionedTextEditorDecorator()
    {
        var editor = new VersionedTextEditor(new TextEditor());

        Assert.Equal(0, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal(string.Empty, editor.Content);

        // Undo without any changes should throw
        Assert.Throws<InvalidOperationException>(editor.Undo);

        // Version 1
        editor.ChangeContent("Version 1");

        Assert.Equal(1, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal("Version 1", editor.Content);

        // Redo without any undo should throw
        Assert.Throws<InvalidOperationException>(editor.Redo);

        // Version 2
        editor.ChangeContent("Version 2");

        Assert.Equal(2, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal("Version 2", editor.Content);

        // Let's undo to version 1
        editor.Undo();

        Assert.Equal(1, editor.UndoCount);
        Assert.Equal(1, editor.RedoCount);
        Assert.Equal("Version 1", editor.Content);

        // Redo to version 2
        editor.Redo();

        Assert.Equal(2, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal("Version 2", editor.Content);

        // Undo again, and this time push a version 3.
        // Version 2 should disappear.
        editor.Undo();
        editor.ChangeContent("Version 3");

        Assert.Equal(2, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal("Version 3", editor.Content);

        // Undo back to a blank state
        editor.Undo();
        editor.Undo();

        Assert.Equal(0, editor.UndoCount);
        Assert.Equal(2, editor.RedoCount);
        Assert.Equal(string.Empty, editor.Content);

        // Redo back to version 3
        editor.Redo();
        editor.Redo();

        Assert.Equal(2, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal("Version 3", editor.Content);
    }
}
