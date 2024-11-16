using CSharpCourse.DesignPatterns.Behavioral.Memento;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.MementoTests;

public class DocumentCaretakerTests
{
    [Fact]
    public void DocumentCaretaker()
    {
        var document = new Document();
        var caretaker = new DocumentCaretaker();

        // Update and save the first version
        document.UpdateContent("First version", "Alice");
        caretaker.SaveVersion(document);

        // Update and save the second version
        document.UpdateContent("Second version", "Bob");
        caretaker.SaveVersion(document);

        // Update some more
        document.UpdateContent("Third version", "Charlie");

        // Go back to the second version, which becomes the current
        // draft version. The third version is lost.
        caretaker.RestoreLastVersion(document);

        Assert.Equal("Second version", document.Content);
        Assert.Equal("Bob", document.Author);

        // We still have the first version in the history
        Assert.Equal(1, caretaker.GetVersionCount());
    }
}
