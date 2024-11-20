namespace CSharpCourse.DesignPatterns.Behavioral.Observer;


// .NET's System namespace already has the IObservable<T> and IObserver<T> interfaces.
// They are heavily used in the Reactive Extensions (Rx) library.

internal class StockPrice : IObservable<decimal>
{
    private readonly List<IObserver<decimal>> _observers = [];

    public IDisposable Subscribe(IObserver<decimal> observer)
    {
        _observers.Add(observer);

        // We make use of an Unsubscriber which, when disposed,
        // removes the observer from the list.
        return new Unsubscriber<decimal>(_observers, observer);
    }

    public void UpdatePrice(decimal price)
        => _observers.ForEach(observer => observer.OnNext(price));
}

internal class Unsubscriber<T> : IDisposable
{
    // This references the same list as the StockPrice class,
    // which we can use to remove the observer from the list.
    private readonly List<IObserver<T>> _observers;
    private readonly IObserver<T> _observer;
    private bool _disposed = false;  // Detect redundant calls

    public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
    {
        _observers = observers;
        _observer = observer;
    }

    // This is the proper way to implement the IDisposable pattern
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        // If already disposed, don't dispose again
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            // Dispose managed
            _observers.Remove(_observer);
        }

        // Dispose unmanaged

        _disposed = true;
    }

    ~Unsubscriber()
    {
        Dispose(false);
    }
}

internal class Observer<T> : IObserver<T>
{
    private readonly Action<T> _onNext;
    private readonly Action<Exception>? _onError;
    private readonly Action? _onCompleted;

    public static IObserver<T> Create(Action<T> onNext,
        Action<Exception>? onError = null, Action? onCompleted = null)
        => new Observer<T>(onNext, onError, onCompleted);

    private Observer(Action<T> onNext, Action<Exception>? onError,
        Action? onCompleted)
    {
        _onNext = onNext;
        _onError = onError;
        _onCompleted = onCompleted;
    }

    public void OnNext(T value) => _onNext(value);
    public void OnError(Exception error) => _onError?.Invoke(error);
    public void OnCompleted() => _onCompleted?.Invoke();
}
