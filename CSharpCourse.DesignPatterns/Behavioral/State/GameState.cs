namespace CSharpCourse.DesignPatterns.Behavioral.State;

// This is an infinite state machine where level 1 is custom
// and the rest of the levels are created dynamically

// State class
internal interface IGameState
{
    // Here we can add all the logic that may be handled differently
    // in each state, so that each state provides its own implementation
    int Level { get; }
    int ScoreThreshold { get; }

    // The state's update method, transitions to other states happen here
    void Update(GameContext context);
}

// Context class that holds the current state
internal class GameContext
{
    // Internal state. Start with level 1
    private IGameState currentState = new Level1State();

    public int Score { get; private set; }
    public int Level => currentState.Level;
    public int ScoreThreshold => currentState.ScoreThreshold;

    public void SetState(IGameState state)
    {
        currentState = state;
    }

    public void Update()
    {
        currentState.Update(this);
    }

    public void IncreaseScore(int points)
    {
        Score += points;
        Console.WriteLine($"Score increased to: {Score}");
    }

    public void NotifyLevelIncrease()
    {
        Console.WriteLine($"Advanced to level: {Level}");
    }
}

internal class Level1State : IGameState
{
    public int ScoreThreshold => (int)Math.Pow(Level, 2) * 1000;
    public int Level => 1;

    public void Update(GameContext context)
    {
        if (context.Score >= ScoreThreshold)
        {
            // Transition to dynamic level state system
            context.SetState(new DynamicLevelState(Level + 1));
            context.NotifyLevelIncrease();
        }
    }
}

internal class DynamicLevelState : IGameState
{
    // Each level requires more points to advance
    public int ScoreThreshold => (int)Math.Pow(Level, 2) * 1000;
    public int Level { get; private set; }

    public DynamicLevelState(int level)
    {
        Level = level;
    }

    public virtual void Update(GameContext context)
    {
        if (context.Score >= ScoreThreshold)
        {
            // Create next level state dynamically
            context.SetState(new DynamicLevelState(Level + 1));
            context.NotifyLevelIncrease();
        }
    }
}
