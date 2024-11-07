using CSharpCourse.DesignPatterns.Behavioral.Memento;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.MementoTests;

public class DinosaurGameTests
{
    [Fact]
    public void Game()
    {
        var game = DinosaurGame.CreateNew();
        game.Run(1000);
        game.TakeDamage();
        game.Run(1000);

        var savedGame = game.Save();
        var previousSpeed = game.Speed;

        // ToArray() is important here, otherwise we wouldn't
        // be enumerating the obstacles until the SequenceEqual
        // call, when the game instance has already changed.
        var obstacles = Enumerable.Repeat(0, 100)
            .Select(_ => game.GetObstacle()).ToArray();

        game.Run(1000);
        game.TakeDamage();
        game.TakeDamage();

        // Game over, let's restore from our save!

        game = DinosaurGame.Load(savedGame);

        Assert.Equal(2, game.Lives);
        Assert.Equal(2000, game.Distance);
        Assert.Equal(previousSpeed, game.Speed);

        var newObstacles = Enumerable.Repeat(0, 100)
            .Select(_ => game.GetObstacle()).ToArray();

        Assert.True(obstacles.SequenceEqual(
            newObstacles));
    }
}
