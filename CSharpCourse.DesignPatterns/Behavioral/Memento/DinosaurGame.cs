namespace CSharpCourse.DesignPatterns.Behavioral.Memento;

// This is our memento.
// We don't save the Random since it can be recreated from the seed.
// We also don't save the speed, since it can be calculated from the distance.
internal record GameState(int Seed, int Lives, long Distance);

internal class DinosaurGame
{
    private readonly int _seed;
    private readonly Random _random;
    public int Lives { get; private set; } = 3;
    public long Distance { get; private set; } = 0;

    // Private constructor
    private DinosaurGame(int seed)
    {
        _seed = seed;
        _random = new Random(seed);
    }

    // Factory method
    public static DinosaurGame CreateNew()
    {
        return new DinosaurGame(Random.Shared.Next());
    }

    public void TakeDamage()
    {
        Lives--;

        if (Lives == 0)
        {
            Console.WriteLine("GAME OVER!");
        }
    }

    public bool GetObstacle()
        => _random.Next(0, 100) < 20;

    public double Speed => Distance / 1000.0;

    public void Run(long distance) => Distance += distance;

    public GameState Save()
        => new(_seed, Lives, Distance);

    public static DinosaurGame Load(GameState state)
        => new(state.Seed) { Lives = state.Lives, Distance = state.Distance };
}
