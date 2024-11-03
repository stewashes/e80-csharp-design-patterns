using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace CSharpCourse.DesignPatterns.Structural.Decorator;

internal record User
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

internal interface IUserRepository
{
    Task<User> GetByIdAsync(int id);
    Task SaveAsync(User user);
}

// This class is sealed, we cannot inherit from it. Many classes in the .NET
// framework are sealed, so we can't inherit from them (e.g., StringBuilder).
// We can also apply the decorator pattern to a class that is not sealed.
internal sealed class UserRepository : IUserRepository
{
    private readonly Dictionary<int, User> _users = [];

    public Task<User> GetByIdAsync(int id)
    {
        if (!_users.TryGetValue(id, out var user))
        {
            throw new KeyNotFoundException($"User with ID {id} not found");
        }

        return Task.FromResult(user);
    }

    public Task SaveAsync(User user)
    {
        _users[user.Id] = user;
        return Task.CompletedTask;
    }
}
