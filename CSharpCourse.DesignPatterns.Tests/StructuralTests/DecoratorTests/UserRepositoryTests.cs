using CSharpCourse.DesignPatterns.Structural.Decorator;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace CSharpCourse.DesignPatterns.Tests.StructuralTests.DecoratorTests;

public class UserRepositoryTests
{
    [Fact]
    public async Task LoggingDecorator()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        var mockLogger = new Mock<ILogger<LoggingUserRepository>>();
        var user = new User { Id = 1, Name = "Test" };
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);
        var decorator = new LoggingUserRepository(mockRepo.Object, mockLogger.Object);

        // Act
        await decorator.GetByIdAsync(1);

        // Assert
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information, // log level
                It.IsAny<EventId>(), // event id
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Fetching user")), // state
                null, // exception
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), // formatter
            Times.Once);
    }

    [Fact]
    public async Task MultipleDecorators()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var validator = new UserValidator();
        var user = new User { Id = 1, Name = "Test" };

        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(user);

        // Decorator chain:
        // ValidationUserRepository -> CachingUserRepository -> mockRepo
        var decorated = new ValidationUserRepository(
            new CachingUserRepository(mockRepo.Object, cache),
            validator);

        // Act
        // Call GetByIdAsync twice to make sure the cache is working
        await decorated.GetByIdAsync(1);
        var result = await decorated.GetByIdAsync(1);

        // Assert
        mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
        Assert.Equal(user, result);
    }

    public record Product
    {
        public int Id { get; init; }
        public required string Name { get; init; }
    }

    [Fact]
    public async Task GenericDecorator()
    {
        // Arrange
        var mockRepo = new Mock<IRepository<Product>>();
        var mockLogger = new Mock<ILogger>();
        var product = new Product { Id = 1, Name = "Test Product" };
        mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(product);

        var decorator = new LoggingRepository<Product>(mockRepo.Object, mockLogger.Object);

        // Act
        await decorator.GetByIdAsync(1);

        // Assert
        mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Product")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Exactly(2));
    }

    [Fact]
    public async Task RetryDecorator()
    {
        // Arrange
        var mockRepo = new Mock<IUserRepository>();
        var callCount = 0;

        mockRepo.Setup(r => r.GetByIdAsync(1))
            .Returns(() =>
            {
                callCount++;

                // On the first call, throw an exception
                if (callCount < 2)
                {
                    throw new Exception("Transient error");
                }

                return Task.FromResult(new User { Id = 1, Name = string.Empty });
            });

        var decorator = new RetryingUserRepository(mockRepo.Object);

        // Act
        var result = await decorator.GetByIdAsync(1);

        // Assert
        Assert.Equal(2, callCount);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void Scrutor()
    {
        var mockLogger = new Mock<ILogger<LoggingUserRepository>>();
        var mockCache = new Mock<IMemoryCache>();

        var services = new ServiceCollection();

        // Register the base implementation
        services.AddScoped<UserRepository>();

        services.AddTransient(sp => mockLogger.Object);
        services.AddTransient(sp => mockCache.Object);
        services.AddTransient<IValidator<User>, UserValidator>();

        services.AddScoped<IUserRepository>(sp => sp.GetRequiredService<UserRepository>())
            .Decorate<IUserRepository, LoggingUserRepository>()
            .Decorate<IUserRepository, CachingUserRepository>()
            .Decorate<IUserRepository, ValidationUserRepository>()
            .Decorate<IUserRepository, RetryingUserRepository>();

        // We could also conditionally add decorators basing on
        // specific environment variables or configuration settings,
        // to enable features.

        var provider = services.BuildServiceProvider();

        var repo = provider.GetRequiredService<IUserRepository>();

        // When we resolve the IUserRepository, we should get the
        // most decorated version of the UserRepository
        Assert.IsType<RetryingUserRepository>(repo);
    }
}
