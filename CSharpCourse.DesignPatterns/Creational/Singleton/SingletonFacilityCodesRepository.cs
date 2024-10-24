namespace CSharpCourse.DesignPatterns.Creational.Singleton;

internal interface IFacilityCodesRepository
{
    bool IsFacilityCodeSupported(string facilityCode);
    IEnumerable<string> GetSupportedFacilityCodes();
    void AddFacilityCode(string facilityCode);
    void RemoveFacilityCode(string facilityCode);
}

#region Classic Singleton
// Singletons should be sealed to prevent inheritance
internal sealed class SingletonFacilityCodesRepository : IFacilityCodesRepository
{
    private static readonly List<string> _facilityCodes = [];

    // We store a single instance, initialized statically,
    // and expose a static getter which will return the instance
    private static readonly SingletonFacilityCodesRepository _instance = new();
    public static SingletonFacilityCodesRepository Instance => _instance;

    private SingletonFacilityCodesRepository()
    {
        // Pretend we load the facility codes from a file
        _facilityCodes.AddRange(["A", "B", "C"]);
    }

    public bool IsFacilityCodeSupported(string facilityCode) => _facilityCodes.Contains(facilityCode);
    public IEnumerable<string> GetSupportedFacilityCodes() => _facilityCodes;
    public void AddFacilityCode(string facilityCode) => _facilityCodes.Add(facilityCode);
    public void RemoveFacilityCode(string facilityCode) => _facilityCodes.Remove(facilityCode);
}
#endregion

#region Lazy Singleton
// This is helpful when the singleton is not always needed, so we delay
// the instantiation until the first time it is requested.
internal sealed class LazySingletonFacilityCodesRepository : IFacilityCodesRepository
{
    private static readonly Lazy<LazySingletonFacilityCodesRepository> _instance = 
        new(() => new LazySingletonFacilityCodesRepository());
    public static LazySingletonFacilityCodesRepository Instance => _instance.Value;

    private readonly List<string> _facilityCodes = [];

    private LazySingletonFacilityCodesRepository()
    {
        // Pretend we load the facility codes from a file
        _facilityCodes.AddRange(["A", "B", "C"]);
    }

    public bool IsFacilityCodeSupported(string facilityCode) => _facilityCodes.Contains(facilityCode);
    public IEnumerable<string> GetSupportedFacilityCodes() => _facilityCodes;
    public void AddFacilityCode(string facilityCode) => _facilityCodes.Add(facilityCode);
    public void RemoveFacilityCode(string facilityCode) => _facilityCodes.Remove(facilityCode);
}
#endregion

#region Singleton with Constructor Parameters
internal sealed class ParameterizedSingletonFacilityCodesRepository : IFacilityCodesRepository
{
    private static ParameterizedSingletonFacilityCodesRepository? _instance;
    private readonly List<string> _facilityCodes;

    private ParameterizedSingletonFacilityCodesRepository(IEnumerable<string> initialCodes)
    {
        _facilityCodes = new List<string>(initialCodes);
    }

    public static ParameterizedSingletonFacilityCodesRepository GetInstance(IEnumerable<string> initialCodes)
    {
        if (_instance is null)
        {
            _instance = new(initialCodes);
        }

        return _instance;
    }

    public bool IsFacilityCodeSupported(string facilityCode) => _facilityCodes.Contains(facilityCode);
    public IEnumerable<string> GetSupportedFacilityCodes() => _facilityCodes;
    public void AddFacilityCode(string facilityCode) => _facilityCodes.Add(facilityCode);
    public void RemoveFacilityCode(string facilityCode) => _facilityCodes.Remove(facilityCode);
}
#endregion

#region Thread-safe Singleton with Double-Checked Locking
internal sealed class ThreadSafeSingletonFacilityCodesRepository : IFacilityCodesRepository
{
    private static volatile ThreadSafeSingletonFacilityCodesRepository? _instance;
    private static readonly object _lock = new();
    private readonly List<string> _facilityCodes = [];

    private ThreadSafeSingletonFacilityCodesRepository()
    {
        _facilityCodes.AddRange(["A", "B", "C"]);
    }

    public static ThreadSafeSingletonFacilityCodesRepository Instance
    {
        get
        {
            // Check once before acquiring the lock, since most of the time
            // the instance will already be created, and locking is expensive
            if (_instance is null)
            {
                lock (_lock)
                {
                    // Check again, since another thread might have created the instance
                    // while we were waiting for the lock
                    if (_instance is null)
                    {
                        _instance = new();
                    }
                }
            }
            return _instance;
        }
    }

    public bool IsFacilityCodeSupported(string facilityCode) => _facilityCodes.Contains(facilityCode);
    public IEnumerable<string> GetSupportedFacilityCodes() => _facilityCodes;
    public void AddFacilityCode(string facilityCode) => _facilityCodes.Add(facilityCode);
    public void RemoveFacilityCode(string facilityCode) => _facilityCodes.Remove(facilityCode);
}
#endregion

#region Inner Static Class Singleton
internal sealed class InnerClassSingletonFacilityCodesRepository : IFacilityCodesRepository
{
    private readonly List<string> _facilityCodes = [];

    private InnerClassSingletonFacilityCodesRepository()
    {
        _facilityCodes.AddRange(["A", "B", "C"]);
    }

    public static InnerClassSingletonFacilityCodesRepository Instance => Nested.InnerInstance;

    private static class Nested
    {
        // The static constructor is guaranteed to be thread-safe by the CLR
        static Nested() { }
        internal static readonly InnerClassSingletonFacilityCodesRepository InnerInstance = new();
    }

    public bool IsFacilityCodeSupported(string facilityCode) => _facilityCodes.Contains(facilityCode);
    public IEnumerable<string> GetSupportedFacilityCodes() => _facilityCodes;
    public void AddFacilityCode(string facilityCode) => _facilityCodes.Add(facilityCode);
    public void RemoveFacilityCode(string facilityCode) => _facilityCodes.Remove(facilityCode);
}
#endregion

#region Async Singleton
internal sealed class AsyncSingletonFacilityCodesRepository : IFacilityCodesRepository
{
    private static AsyncSingletonFacilityCodesRepository? _instance;

    // We use a semaphore instead of a lock, since we are in the async world
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly List<string> _facilityCodes = [];

    private AsyncSingletonFacilityCodesRepository() { }

    // We use a ValueTask to avoid unnecessary allocations, since
    // most of the time the instance will already be created
    public static async ValueTask<AsyncSingletonFacilityCodesRepository> GetInstanceAsync()
    {
        // Again, this is a thread-safe double-checked locking pattern
        if (_instance is null)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (_instance is null)
                {
                    _instance = new();
                    await _instance.InitializeAsync();
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
        return _instance;
    }

    private async Task InitializeAsync()
    {
        // Simulate async initialization, e.g., loading from a database
        await Task.Delay(100);
        _facilityCodes.AddRange(["A", "B", "C"]);
    }

    public bool IsFacilityCodeSupported(string facilityCode) => _facilityCodes.Contains(facilityCode);
    public IEnumerable<string> GetSupportedFacilityCodes() => _facilityCodes;
    public void AddFacilityCode(string facilityCode) => _facilityCodes.Add(facilityCode);
    public void RemoveFacilityCode(string facilityCode) => _facilityCodes.Remove(facilityCode);
}
#endregion

#region Optimized Async Singleton
internal sealed class OptimizedAsyncSingletonFacilityCodesRepository : IFacilityCodesRepository
{
    private static OptimizedAsyncSingletonFacilityCodesRepository? _instance;
    private static Task<OptimizedAsyncSingletonFacilityCodesRepository>? _initializationTask;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private readonly List<string> _facilityCodes = [];

    private OptimizedAsyncSingletonFacilityCodesRepository() { }

    // ValueTask can only be awaited once, so we need to return a new instance
    // every time it's requested (but it's a value type, so it's cheap)
    public static ValueTask<OptimizedAsyncSingletonFacilityCodesRepository> GetInstanceAsync()
    {
        if (_instance is not null)
        {
            return new ValueTask<OptimizedAsyncSingletonFacilityCodesRepository>(_instance);
        }

        return new ValueTask<OptimizedAsyncSingletonFacilityCodesRepository>(GetInstanceAsyncInternal());
    }

    private static async Task<OptimizedAsyncSingletonFacilityCodesRepository> GetInstanceAsyncInternal()
    {
        if (_initializationTask is null)
        {
            // Reduced contention: we only use the semaphore if the
            // initialization task is not already running
            await _semaphore.WaitAsync();
            try
            {
                if (_initializationTask is null)
                {
                    _initializationTask = InitializeAsync();
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        return await _initializationTask;
    }

    private static async Task<OptimizedAsyncSingletonFacilityCodesRepository> InitializeAsync()
    {
        var instance = new OptimizedAsyncSingletonFacilityCodesRepository();
        // Simulate async initialization, e.g., loading from a database
        await Task.Delay(100);
        instance._facilityCodes.AddRange(["A", "B", "C"]);
        _instance = instance;
        return instance;
    }

    public bool IsFacilityCodeSupported(string facilityCode) => _facilityCodes.Contains(facilityCode);
    public IEnumerable<string> GetSupportedFacilityCodes() => _facilityCodes;
    public void AddFacilityCode(string facilityCode) => _facilityCodes.Add(facilityCode);
    public void RemoveFacilityCode(string facilityCode) => _facilityCodes.Remove(facilityCode);
}
#endregion

#region Monostate Pattern
// It is possible to just use a static class, but then we cannot
// even customize the constructor. This is a variation of the singleton
// pattern where we have a static class with static fields, but we can
// still have a constructor.
internal class MonostateFacilityCodesRepository : IFacilityCodesRepository
{
    private static readonly List<string> _facilityCodes = [];

    static MonostateFacilityCodesRepository()
    {
        // Only load the facility codes if they are not already loaded
        if (_facilityCodes.Count == 0)
        {
            _facilityCodes.AddRange(["A", "B", "C"]);
        }
    }

    public bool IsFacilityCodeSupported(string facilityCode) => _facilityCodes.Contains(facilityCode);
    public IEnumerable<string> GetSupportedFacilityCodes() => _facilityCodes;
    public void AddFacilityCode(string facilityCode) => _facilityCodes.Add(facilityCode);
    public void RemoveFacilityCode(string facilityCode) => _facilityCodes.Remove(facilityCode);
}
#endregion

#region Dependency Injection
// Normal class without any singleton-specific behavior
internal class FacilityCodesRepository : IFacilityCodesRepository
{
    private readonly List<string> _facilityCodes;

    public FacilityCodesRepository()
    {
        _facilityCodes = ["A", "B", "C"];
    }

    public bool IsFacilityCodeSupported(string facilityCode) => _facilityCodes.Contains(facilityCode);
    public IEnumerable<string> GetSupportedFacilityCodes() => _facilityCodes;
    public void AddFacilityCode(string facilityCode) => _facilityCodes.Add(facilityCode);
    public void RemoveFacilityCode(string facilityCode) => _facilityCodes.Remove(facilityCode);
}

// This is a service that uses the repository
internal class FacilityManager
{
    private readonly IFacilityCodesRepository _repository;

    public FacilityManager(IFacilityCodesRepository repository)
    {
        _repository = repository;
    }

    public string GetLastCode()
    {
        var codes = _repository.GetSupportedFacilityCodes().ToList();

        if (codes.Count == 0)
        {
            throw new InvalidOperationException("No facility codes available");
        }

        return codes[^1];
    }
}
#endregion
