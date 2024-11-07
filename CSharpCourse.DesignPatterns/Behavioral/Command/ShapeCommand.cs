namespace CSharpCourse.DesignPatterns.Behavioral.Command;

internal class Shape()
{
    public double Size { get; set; } = 1;
    public string Color { get; set; } = "Black";
    public int PositionX { get; set; } = 0;
    public int PositionY { get; set; } = 0;
}

internal interface ICommand
{
    void Do();
    void Undo();
}

// We use an abstract class here since we want to enforce setting
// the shape as a constructor parameter in subtypes
internal abstract class ShapeCommand
{
    readonly protected Shape _shape;

    protected ShapeCommand(Shape shape)
    {
        _shape = shape;
    }
}

internal class ResizeCommand : ShapeCommand, ICommand
{
    public double ScalingFactor { get; }

    public ResizeCommand(Shape shape, double scalingFactor) : base(shape)
    {
        ScalingFactor = scalingFactor;
    }

    public void Do()
    {
        _shape.Size *= ScalingFactor;
    }

    public void Undo()
    {
        _shape.Size /= ScalingFactor;
    }
}

internal class ChangeColorCommand : ShapeCommand, ICommand
{
    public string Color { get; }
    private string _previousColor = "White";

    public ChangeColorCommand(Shape shape, string color) : base(shape)
    {
        Color = color;
    }

    public void Do()
    {
        _previousColor = _shape.Color;
        _shape.Color = Color;
    }

    public void Undo()
    {
        _shape.Color = _previousColor;
    }
}

internal class MoveCommand : ShapeCommand, ICommand
{
    public int MoveX { get; }
    public int MoveY { get; }

    public MoveCommand(Shape shape, int moveX, int moveY) : base(shape)
    {
        MoveX = moveX;
        MoveY = moveY;
    }

    public void Do()
    {
        _shape.PositionX += MoveX;
        _shape.PositionY += MoveY;
    }

    public void Undo()
    {
        _shape.PositionX -= MoveX;
        _shape.PositionY -= MoveY;
    }
}

internal class MacroCommand : ICommand
{
    private readonly ICommand[] _commands;

    public MacroCommand(params ICommand[] commands)
    {
        _commands = commands;
    }

    public void Do()
    {
        foreach (var command in _commands)
        {
            command.Do();
        }
    }

    public void Undo()
    {
        foreach (var command in _commands.Reverse())
        {
            command.Undo();
        }
    }
}

internal class CommandHistory
{
    private readonly Stack<ICommand> _undoStack = new();
    private readonly Stack<ICommand> _redoStack = new();

    public void Execute(ICommand command)
    {
        command.Do();
        _undoStack.Push(command);
        _redoStack.Clear();
    }

    public void Undo()
    {
        if (_undoStack.Count == 0)
        {
            return;
        }

        var command = _undoStack.Pop();
        command.Undo();
        _redoStack.Push(command);
    }

    public void Redo()
    {
        if (_redoStack.Count == 0)
        {
            return;
        }

        var command = _redoStack.Pop();
        command.Do();
        _undoStack.Push(command);
    }
}
