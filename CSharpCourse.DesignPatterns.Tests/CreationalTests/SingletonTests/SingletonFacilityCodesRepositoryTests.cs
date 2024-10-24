using CSharpCourse.DesignPatterns.Creational.Singleton;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;

namespace CSharpCourse.DesignPatterns.Tests.CreationalTests.SingletonTests;

public class SingletonFacilityCodesRepositoryTests
{
    [Fact]
    public void ClassicSingleton()
    {
        var repository = SingletonFacilityCodesRepository.Instance;
        var repository2 = SingletonFacilityCodesRepository.Instance;

        // Make sure we have the same instance
        Assert.Same(repository, repository2);
    }

    [Fact]
    public void LazySingleton()
    {
        var repository = LazySingletonFacilityCodesRepository.Instance;
        var repository2 = LazySingletonFacilityCodesRepository.Instance;

        // Make sure we have the same instance
        Assert.Same(repository, repository2);
    }

    [Fact]
    public void ParameterizedSingleton()
    {
        var facilityCodes = new List<string> { "A", "B", "C" };
        var repository = ParameterizedSingletonFacilityCodesRepository.GetInstance(facilityCodes);
        var repository2 = ParameterizedSingletonFacilityCodesRepository.GetInstance(facilityCodes);

        // Make sure we have the same instance
        Assert.Same(repository, repository2);

        // Make sure the initial codes are there
        Assert.True(facilityCodes.SequenceEqual(repository.GetSupportedFacilityCodes()));
        Assert.True(facilityCodes.SequenceEqual(repository2.GetSupportedFacilityCodes()));
    }

    [Fact]
    public void ThreadSafeSingleton()
    {
        var instance1 = ThreadSafeSingletonFacilityCodesRepository.Instance;
        var instance2 = ThreadSafeSingletonFacilityCodesRepository.Instance;

        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void InnerClassSingleton()
    {
        var instance1 = InnerClassSingletonFacilityCodesRepository.Instance;
        var instance2 = InnerClassSingletonFacilityCodesRepository.Instance;

        Assert.Same(instance1, instance2);
    }

    [Fact]
    public async Task AsyncSingleton()
    {
        AsyncSingletonFacilityCodesRepository? instance1 = null;
        AsyncSingletonFacilityCodesRepository? instance2 = null;

        var task1 = Task.Run(async () => { 
            instance1 = await AsyncSingletonFacilityCodesRepository.GetInstanceAsync();
        });
        var task2 = Task.Run(async () => {
            instance2 = await AsyncSingletonFacilityCodesRepository.GetInstanceAsync();
        });

        await Task.WhenAll(task1, task2);

        Assert.NotNull(instance1);
        Assert.NotNull(instance2);
        Assert.Same(instance1, instance2);
    }

    [Fact]
    public async Task OptimizedAsyncSingleton()
    {
        OptimizedAsyncSingletonFacilityCodesRepository? instance1 = null;
        OptimizedAsyncSingletonFacilityCodesRepository? instance2 = null;

        var task1 = Task.Run(async () => {
            instance1 = await OptimizedAsyncSingletonFacilityCodesRepository.GetInstanceAsync();
        });
        var task2 = Task.Run(async () => {
            instance2 = await OptimizedAsyncSingletonFacilityCodesRepository.GetInstanceAsync();
        });

        await Task.WhenAll(task1, task2);

        Assert.NotNull(instance1);
        Assert.NotNull(instance2);
        Assert.Same(instance1, instance2);
    }

    [Fact]
    public void Monostate()
    {
        var repository = new MonostateFacilityCodesRepository();
        repository.AddFacilityCode("D");

        var repository2 = new MonostateFacilityCodesRepository();

        // Different instances, but they share the same state
        Assert.NotSame(repository, repository2);
        Assert.Contains("D", repository2.GetSupportedFacilityCodes());
    }

    // Testability issue: if a component uses the singleton like
    // SingletonFacilityCodesRepository.Instance, it is hard to test on
    // a unit level.
    //
    // We can't mock the repository, and we can't replace
    // the instance with a mock. We can't even create a new instance
    // for each test. This is a problem with the singleton pattern.
    //
    // This is why, if we want to use a singleton, it is better to use
    // dependency injection to inject the abstract singleton instance.
    // This is the "accepted" way to use a singleton.

    [Fact]
    public void DependencyInjection()
    {
        // We use the Microsoft.Extensions.DependencyInjection library

        // Setup DI
        var services = new ServiceCollection();
        services.AddSingleton<IFacilityCodesRepository, FacilityCodesRepository>();
        services.AddTransient<FacilityManager>();
        var serviceProvider = services.BuildServiceProvider(); // DI Container

        // Test FacilityCodesRepository Singleton behavior
        var repo1 = serviceProvider.GetRequiredService<IFacilityCodesRepository>();
        var repo2 = serviceProvider.GetRequiredService<IFacilityCodesRepository>();
        Assert.Same(repo1, repo2);

        // Test FacilityManager Transient behavior
        var manager1 = serviceProvider.GetRequiredService<FacilityManager>();
        var manager2 = serviceProvider.GetRequiredService<FacilityManager>();
        Assert.NotSame(manager1, manager2);

        // Test that FacilityManagers share the same repository
        repo1.AddFacilityCode("D");

        var manager1Code = manager1.GetLastCode();
        var manager2Code = manager2.GetLastCode();

        // Both managers should have the same last code, "D"
        Assert.Equal("D", manager1Code);
        Assert.Equal("D", manager2Code);
    }
}
