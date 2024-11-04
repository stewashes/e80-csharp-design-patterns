using CSharpCourse.DesignPatterns.Structural.Flyweight;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.FlyweightTests;

public class GlyphSharingTests
{
    [Fact]
    public void TestGlyphSharing()
    {
        var factory = new GlyphFactory();
        List<Character> document = [];

        // Simulate a document with repeated characters
        string content = "AAABBBCCC";
        int positionX = 0;
        int positionY = 0;

        foreach (char c in content)
        {
            Glyph glyph = factory.GetGlyph(c, "Arial", 12);
            var character = new Character(glyph, positionX++, positionY);
            document.Add(character);
        }

        // Draw the document
        foreach (var character in document)
        {
            character.Draw();
        }

        // Assert that only 3 glyphs were created
        Assert.Equal(3, factory.GlyphCount);
    }
}
