using CSharpCourse.DesignPatterns.Behavioral.State;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.StateTests;

public class GameStateTests
{
    [Fact]
    public void GameState()
    {
        var game = new GameContext();

        Assert.Equal(1, game.Level);
        Assert.Equal(0, game.Score);

        game.IncreaseScore(1000);
        game.Update();

        Assert.Equal(2, game.Level);
        Assert.Equal(1000, game.Score);

        game.IncreaseScore(3000);
        game.Update();

        Assert.Equal(3, game.Level);
        Assert.Equal(4000, game.Score);
    }
}
