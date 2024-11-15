using CSharpCourse.DesignPatterns.Behavioral.State;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.StateTests;

public class EnemyTests
{
    [Fact]
    public async Task Enemy()
    {
        var enemy = new Enemy
        {
            IdleInterval = TimeSpan.FromMilliseconds(10),
            AlertInterval = TimeSpan.FromMilliseconds(10),
        };

        // The initial state is idle
        Assert.Equal(EnemyState.Idle, enemy.State);

        await Task.Delay(20);
        enemy.Update();

        // After the idle interval, the state should be patrol
        Assert.Equal(EnemyState.Patrol, enemy.State);

        // There is a suspicious sound near the enemy
        enemy.NotifySound();

        // The state should be alert
        Assert.Equal(EnemyState.Alert, enemy.State);

        await Task.Delay(20);
        enemy.Update();

        // After the alert interval, the state should be patrol
        Assert.Equal(EnemyState.Patrol, enemy.State);

        // The player enters the enemy's cone of vision
        enemy.PlayerInView = true;
        enemy.Update();

        // The state should be engage in combat
        Assert.Equal(EnemyState.EngageInCombat, enemy.State);
    }
}
