using CSharpCourse.DesignPatterns.Behavioral.Command;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.CommandTests;

public class ShapeCommandTests
{
    [Fact]
    public void SingleCommand()
    {
        var shape = new Shape()
        {
            Size = 1,
            Color = "Red",
            PositionX = 0,
            PositionY = 0
        };

        var resizeCommand = new ResizeCommand(shape, 2);
        resizeCommand.Do();

        Assert.Equal(2, shape.Size);

        resizeCommand.Undo();

        Assert.Equal(1, shape.Size);
    }

    [Fact]
    public void Macro()
    {
        var shape = new Shape()
        {
            Size = 1,
            Color = "Red",
            PositionX = 0,
            PositionY = 0
        };

        var macro = new MacroCommand(
                new ResizeCommand(shape, 2),
                new ChangeColorCommand(shape, "Blue"),
                new MoveCommand(shape, 10, 20)
            );

        macro.Do();

        Assert.Equal(2, shape.Size);
        Assert.Equal("Blue", shape.Color);
        Assert.Equal(10, shape.PositionX);
        Assert.Equal(20, shape.PositionY);

        macro.Undo();

        Assert.Equal(1, shape.Size);
        Assert.Equal("Red", shape.Color);
        Assert.Equal(0, shape.PositionX);
        Assert.Equal(0, shape.PositionY);
    }

    [Fact]
    public void History()
    {
        var shape = new Shape()
        {
            Size = 1,
            Color = "Red",
            PositionX = 0,
            PositionY = 0
        };

        var history = new CommandHistory();

        var resizeCommand = new ResizeCommand(shape, 2);
        var changeColorCommand = new ChangeColorCommand(shape, "Blue");

        history.Execute(resizeCommand);
        Assert.Equal(2, shape.Size);

        history.Execute(changeColorCommand);
        Assert.Equal("Blue", shape.Color);

        history.Undo();
        Assert.Equal("Red", shape.Color);

        history.Redo();
        Assert.Equal("Blue", shape.Color);
    }
}
