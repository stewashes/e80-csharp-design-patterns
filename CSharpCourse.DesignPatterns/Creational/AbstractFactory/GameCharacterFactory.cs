using System.Reflection;

namespace CSharpCourse.DesignPatterns.Creational.Factory;

// This is not the textbook example of the Abstract Factory pattern,
// but it is something that is actually useful in the real world.

#region Public classes
// This is part of our library's API. The consumer of the library will only see
// these interfaces and classes. They will not see the internal classes.
public interface IGameCharacter
{
    void Spawn();
}

public interface ICharacterFactory
{
    IGameCharacter Create(int level);
}
#endregion

#region Internal classes
// Internal classes, not given out to the consumer of the library.
internal class Ninja : IGameCharacter
{
    public void Spawn()
        => Console.WriteLine("Ninja appears in a puff of smoke!");
}

internal class Dragon : IGameCharacter
{
    public void Spawn()
        => Console.WriteLine("Dragon swoops down with a mighty roar!");
}

internal class NinjaFactory : ICharacterFactory
{
    public IGameCharacter Create(int level)
    {
        Console.WriteLine($"Spawning Ninja at level {level}");
        return new Ninja();
    }
}

internal class DragonFactory : ICharacterFactory
{
    public IGameCharacter Create(int level)
    {
        Console.WriteLine($"Spawning Dragon at level {level}");
        return new Dragon();
    }
}
#endregion

#region Game character factory
public static class GameCharacterFactory
{
    // This breaks the Open/Closed Principle
    public enum CharacterType
    {
        Ninja,
        Dragon
    }

    private readonly static Dictionary<CharacterType, ICharacterFactory>
        _factories = new()
        {
            [CharacterType.Ninja] = new NinjaFactory(),
            [CharacterType.Dragon] = new DragonFactory()
        };

    public static IGameCharacter Create(CharacterType type, int level)
    {
        return _factories[type].Create(level);
    }
}
#endregion

#region Open/Closed-compliant factory
public class OccGameCharacterFactory
{
    private readonly Dictionary<string, ICharacterFactory> _factories;

    public OccGameCharacterFactory()
    {
        // Case-insensitive dictionary
        _factories = new(StringComparer.OrdinalIgnoreCase);

        // We use reflection to find all types that implement ICharacterFactory
        // in the current assembly, and automatically register them.
        // Reflection is expensive when done at run-time, but in this case
        // it's done once at start-up, so it's not a problem.
        foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
        {
            if (typeof(ICharacterFactory).IsAssignableFrom(type) && !type.IsInterface)
            {
                var factory = (ICharacterFactory)Activator.CreateInstance(type)!;

                // Trade-off: We are using the type's name as the key.
                // This is a convention-based approach. It might not always
                // be the best approach, but it's simple and works in many cases.
                // Make sure to write extensive tests that will break if
                // the name of a class changes.
                RegisterFactory(type.Name.Replace("Factory", ""), factory);
            }
        }
    }

    private void RegisterFactory(string type, ICharacterFactory factory)
    {
        _factories[type] = factory;
    }

    public IGameCharacter Create(string characterType, int level)
    {
        return _factories[characterType].Create(level);
    }
}
#endregion
