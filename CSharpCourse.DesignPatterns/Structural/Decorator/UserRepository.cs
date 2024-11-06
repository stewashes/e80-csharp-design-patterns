using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace CSharpCourse.DesignPatterns.Structural.Decorator;

// Decorators are usually built for purposes such as adding
// - logging
// - caching
// - retry logic
// - security
// - transaction management
// - validation
// - monitoring
// etc...

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

// We don't necessarily have to implement the same interface as
// the original repository, but it's a good practice to do so
// to avoid changing client code.

// Also, we must make sure to not break the Liskov Substitution Principle.

#region Logging decorator
internal class LoggingUserRepository : IUserRepository
{
    private readonly IUserRepository _repository;

    // We use the Microsoft.Extensions.Logging.Abstractions package to log messages
    private readonly ILogger<LoggingUserRepository> _logger;

    public LoggingUserRepository(IUserRepository repository, ILogger<LoggingUserRepository> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        // We decorate the original GetByIdAsync method with logging
        _logger.LogInformation("Fetching user with ID: {Id}", id);
        var result = await _repository.GetByIdAsync(id);
        _logger.LogInformation("Successfully fetched user: {Id}", id);
        return result;
    }

    public async Task SaveAsync(User user)
    {
        _logger.LogInformation("Saving user: {Id}", user.Id);
        await _repository.SaveAsync(user);
        _logger.LogInformation("Successfully saved user: {Id}", user.Id);
    }
}
#endregion

#region Caching decorator
internal class CachingUserRepository : IUserRepository
{
    private readonly IUserRepository _repository;

    // We use the Microsoft.Extensions.Caching.Abstractions package to cache objects
    private readonly IMemoryCache _cache;
    private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(10);

    public CachingUserRepository(IUserRepository repository, IMemoryCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        var cacheKey = $"user_{id}";

        if (_cache.TryGetValue<User>(cacheKey, out var user))
        {
            return user!;
        }

        user = await _repository.GetByIdAsync(id);
        _cache.Set(cacheKey, user, _cacheExpiration);
        return user;
    }

    public async Task SaveAsync(User user)
    {
        await _repository.SaveAsync(user);
        _cache.Remove($"user_{user.Id}");
    }
}
#endregion

#region Validation decorator
internal class ValidationUserRepository : IUserRepository
{
    private readonly IUserRepository _repository;

    // We use the FluentValidation package to validate objects
    private readonly IValidator<User> _validator;

    public ValidationUserRepository(IUserRepository repository, IValidator<User> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public Task<User> GetByIdAsync(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException("Invalid user ID", nameof(id));
        }

        return _repository.GetByIdAsync(id);
    }

    public async Task SaveAsync(User user)
    {
        var validationResult = await _validator.ValidateAsync(user);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        await _repository.SaveAsync(user);
    }
}

internal class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Id).GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty();
    }
}
#endregion

#region Generic decorator
internal interface IRepository<T> where T : class
{
    Task<T> GetByIdAsync(int id);
    Task SaveAsync(T entity);
}

// This decorator adds logging to a generic repository
internal class LoggingRepository<T> : IRepository<T> where T : class
{
    private readonly IRepository<T> _repository;
    private readonly ILogger _logger;

    public LoggingRepository(IRepository<T> repository, ILogger logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<T> GetByIdAsync(int id)
    {
        _logger.LogInformation("Fetching {Type} with ID: {Id}", typeof(T).Name, id);
        var result = await _repository.GetByIdAsync(id);
        _logger.LogInformation("Fetched {Type}: {Id}", typeof(T).Name, id);
        return result;
    }

    public async Task SaveAsync(T entity)
    {
        _logger.LogInformation("Saving {Type}", typeof(T).Name);
        await _repository.SaveAsync(entity);
        _logger.LogInformation("Saved {Type}", typeof(T).Name);
    }
}
#endregion

#region Retry decorator
internal class RetryingUserRepository : IUserRepository
{
    private readonly IUserRepository _repository;

    // We use the Polly package to add retry logic
    private readonly AsyncRetryPolicy _retryPolicy;

    public RetryingUserRepository(
        IUserRepository repository)
    {
        _repository = repository;
        _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public Task<User> GetByIdAsync(int id) =>
        _retryPolicy.ExecuteAsync(() => _repository.GetByIdAsync(id));

    public Task SaveAsync(User user) =>
        _retryPolicy.ExecuteAsync(() => _repository.SaveAsync(user));
}
#endregion
