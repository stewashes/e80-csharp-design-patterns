using System.Diagnostics;

namespace CSharpCourse.DesignPatterns.Behavioral.State;

// If we want to add behavior in this case we violate the
// Open/Closed Principle. We should instead use separate state classes.

internal enum EnemyState
{
    Idle,
    Patrol,
    Alert,
    EngageInCombat
}

internal class Enemy
{
    public EnemyState State { get; private set; } = EnemyState.Idle;
    private readonly Stopwatch _idleTimer = Stopwatch.StartNew();
    private readonly Stopwatch _alertTimer = new();

    public TimeSpan IdleInterval { get; init; } = TimeSpan.FromSeconds(5);
    public TimeSpan AlertInterval { get; init; } = TimeSpan.FromSeconds(5);

    // These values should be computed here but we mock them
    // for simplicity.
    public bool PlayerInView { get; set; }
    public int PlayerHealth { get; set; } = 100;
    public bool StandingOnWaypoint { get; set; }

    // Function called on each frame (e.g., in the Unity game engine)
    // by the main thread.
    public void Update()
    {
        switch (State)
        {
            case EnemyState.Idle:
                // If idle for more than N seconds, patrol
                if (_idleTimer.Elapsed > IdleInterval)
                {
                    _idleTimer.Stop();
                    State = EnemyState.Patrol;
                }
                break;

            case EnemyState.Patrol:
                // Move towards next waypoint in the patrol path

                // When we reach a waypoint, stay idle for a bit
                if (WaypointReached())
                {
                    State = EnemyState.Idle;
                    _idleTimer.Restart();
                }
                break;

            case EnemyState.Alert:
                // Move towards the source of the sound

                // If alerted for more than N seconds, go back to patrol
                if (_alertTimer.Elapsed > AlertInterval)
                {
                    _alertTimer.Stop();
                    State = EnemyState.Patrol;
                }
                break;

            case EnemyState.EngageInCombat:
                if (!PlayerIsDead())
                {
                    // Shoot the player!
                }
                else
                {
                    // Otherwise, go back to patrol
                    // in the game over screen.
                    State = EnemyState.Patrol;
                }
                break;
        }

        // If not in combat, scout for the player. If we see
        // the player, engage in combat.
        if (State is not EnemyState.EngageInCombat)
        {
            ScoutForPlayer();
        }
    }

    // Event, called by the sound source around a given radius
    public void NotifySound()
    {
        if (State is not EnemyState.EngageInCombat)
        {
            State = EnemyState.Alert;
            _alertTimer.Restart();
        }
    }

    private void ScoutForPlayer()
    {
        // Check if the player is in the view cone of the enemy
        if (CanSeePlayer())
        {
            State = EnemyState.EngageInCombat;
        }
    }

    private bool CanSeePlayer() => PlayerInView;
    private bool WaypointReached() => StandingOnWaypoint;
    private bool PlayerIsDead() => PlayerHealth <= 0;
}
