namespace CSharpCourse.DesignPatterns.Structural.Flyweight;

// This is a sample use case where the flyweight pattern is used
// to share the same glyph object for repeated characters in a document.
// It is much lighter to store a pointer to a shared object
// than to store the object itself multiple times.

// The same could be applied for example to a game engine where the same
// texture is used multiple times in the same scene.

// Flyweight class
internal record Glyph
{
    public char Symbol { get; private set; }
    public string Font { get; private set; }
    public int Size { get; private set; }

    public Glyph(char symbol, string font, int size)
    {
        Symbol = symbol;
        Font = font;
        Size = size;
    }

    public void Draw(int x, int y)
    {
        Console.WriteLine($"Drawing '{Symbol}' at ({x},{y}) with font '{Font}' and size {Size}");
    }
}

// Flyweight factory
internal class GlyphFactory
{
    private readonly Dictionary<string, Glyph> _glyphs = [];

    public Glyph GetGlyph(char symbol, string font, int size)
    {
        string key = $"{symbol}_{font}_{size}";

        if (!_glyphs.TryGetValue(key, out Glyph? value))
        {
            value = new Glyph(symbol, font, size);
            _glyphs[key] = value;
        }

        return value;
    }

    public int GlyphCount => _glyphs.Count;
}

// Context class
internal class Character
{
    private readonly Glyph _glyph;
    private readonly int _positionX;
    private readonly int _positionY;

    public Character(Glyph glyph, int positionX, int positionY)
    {
        _glyph = glyph;
        _positionX = positionX;
        _positionY = positionY;
    }

    public void Draw()
    {
        _glyph.Draw(_positionX, _positionY);
    }
}
