using CSharpCourse.DesignPatterns.Assignments;

namespace CSharpCourse.DesignPatterns.Tests.AssignmentTests;

public class TextEditorMementoTests
{
    [Fact]
    public void TextEditor()
    {
        var editor = new VersionedTextEditor();

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

    [Fact]
    public void LimitedTextEditor()
    {
        var editor = new LimitedVersionedTextEditor(3);

        Assert.Equal(0, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal(string.Empty, editor.Content);

        // Undo without any changes should throw
        Assert.Throws<InvalidOperationException>(editor.Undo);

        // Add version 1
        editor.ChangeContent("Version 1");
        Assert.Equal(1, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal("Version 1", editor.Content);

        // Redo without any undo should throw
        Assert.Throws<InvalidOperationException>(editor.Redo);

        // Add version 2 (buffer is now full)
        editor.ChangeContent("Version 2");
        Assert.Equal(2, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal("Version 2", editor.Content);

        // Add version 3 (overwrites the empty string)
        editor.ChangeContent("Version 3");
        Assert.Equal(2, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal("Version 3", editor.Content);

        // Add version 4 (should overwrite version 1)
        editor.ChangeContent("Version 4");
        Assert.Equal(2, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal("Version 4", editor.Content);

        // Undo to version 3
        editor.Undo();
        Assert.Equal(1, editor.UndoCount);
        Assert.Equal(1, editor.RedoCount);
        Assert.Equal("Version 3", editor.Content);

        // Undo to version 2
        editor.Undo();
        Assert.Equal(0, editor.UndoCount);
        Assert.Equal(2, editor.RedoCount);
        Assert.Equal("Version 2", editor.Content);

        // Cannot undo further as version 1 was overwritten
        Assert.Throws<InvalidOperationException>(editor.Undo);

        // Redo to version 3
        editor.Redo();
        Assert.Equal(1, editor.UndoCount);
        Assert.Equal(1, editor.RedoCount);
        Assert.Equal("Version 3", editor.Content);

        // Redo to version 4
        editor.Redo();
        Assert.Equal(2, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal("Version 4", editor.Content);

        // Go back to version 2
        editor.Undo();
        editor.Undo();
        Assert.Equal(0, editor.UndoCount);
        Assert.Equal(2, editor.RedoCount);

        // Add version 5, this should clear the redo stack
        editor.ChangeContent("Version 5");
        Assert.Equal(1, editor.UndoCount);
        Assert.Equal(0, editor.RedoCount);
        Assert.Equal("Version 5", editor.Content);

        // Verify we can still undo to version 2
        editor.Undo();
        Assert.Equal(0, editor.UndoCount);
        Assert.Equal(1, editor.RedoCount);
        Assert.Equal("Version 2", editor.Content);
    }
}
