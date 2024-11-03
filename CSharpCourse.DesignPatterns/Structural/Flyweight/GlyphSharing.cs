using static System.Net.Mime.MediaTypeNames;

namespace CSharpCourse.DesignPatterns.Structural.Flyweight;

internal class Character
{
    private readonly char _symbol;
    private readonly string _font;
    private readonly int _size;
    private readonly int _positionX;
    private readonly int _positionY;

    public Character(char symbol, string font, int size, int positionX, int positionY)
    {
        _symbol = symbol;
        _font = font;
        _size = size;
        _positionX = positionX;
        _positionY = positionY;
    }

    public void Draw()
    {
        Console.WriteLine($"Drawing '{_symbol}' at ({_positionX},{_positionY}) with font '{_font}' and size {_size}");
    }
}
