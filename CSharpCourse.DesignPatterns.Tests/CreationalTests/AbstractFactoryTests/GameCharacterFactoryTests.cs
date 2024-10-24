using CSharpCourse.DesignPatterns.Creational.Factory;
using CSharpCourse.DesignPatterns.Tests.Utils;

namespace CSharpCourse.DesignPatterns.Tests.CreationalTests.AbstractFactoryTests;

public class GameCharacterFactoryTests
{
    [Fact]
    public void AbstractFactory()
    {
        var ninja = GameCharacterFactory.Create(
            GameCharacterFactory.CharacterType.Ninja, 1);
        var output = OutputUtils.CaptureConsoleOutput(ninja.Spawn);
        Assert.Contains("Ninja appears in a puff of smoke!", output);

        var dragon = GameCharacterFactory.Create(
            GameCharacterFactory.CharacterType.Dragon, 1);
        output = OutputUtils.CaptureConsoleOutput(dragon.Spawn);
        Assert.Contains("Dragon swoops down with a mighty roar!", output);
    }

    [Fact]
    public void OpenCloseCompliantFactory()
    {
        var factory = new OccGameCharacterFactory();
        
        var ninja = factory.Create("Ninja", 1);
        var output = OutputUtils.CaptureConsoleOutput(ninja.Spawn);
        Assert.Contains("Ninja appears in a puff of smoke!", output);

        // Check case-insensitivity
        var dragon = factory.Create("dragon", 1);
        output = OutputUtils.CaptureConsoleOutput(dragon.Spawn);
        Assert.Contains("Dragon swoops down with a mighty roar!", output);
    }
}
