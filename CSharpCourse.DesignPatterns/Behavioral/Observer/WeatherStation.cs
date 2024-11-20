namespace CSharpCourse.DesignPatterns.Behavioral.Observer;

// T is contravariant, which means that the type parameter can
// of a more general type than originally specified.
internal interface IListener<in T>
{
    void Update(T data);
}

internal interface ISubject<T>
{
    void Attach(IListener<T> observer);
    void Detach(IListener<T> observer);
    void Notify(T data);
}

internal class WeatherStation : ISubject<double>
{
    private readonly List<IListener<double>> _observers = [];
    public double Temperature { get; private set; }

    public void SetTemperature(double temperature)
    {
        Temperature = temperature;
        Notify(Temperature);
    }

    public void Attach(IListener<double> observer) => _observers.Add(observer);
    public void Detach(IListener<double> observer) => _observers.Remove(observer);
    public void Notify(double data) => _observers.ForEach(o => o.Update(data));
}

internal class TemperatureLogger : IListener<double>
{
    public void Update(double temperature)
    {
        Console.WriteLine($"Temperature at {DateTime.Now}: {temperature} degrees Celsius");
    }
}

internal class TemperatureNotifier : IListener<double>
{
    private bool _lastSampleBelowZero;

    public void Update(double temperature)
    {
        // We only want to notify once when the temperature goes below zero
        if (temperature < 0 && !_lastSampleBelowZero)
        {
            Console.WriteLine("Temperature is below zero!");
            _lastSampleBelowZero = true;
        }
        else if (temperature >= 0 && _lastSampleBelowZero)
        {
            _lastSampleBelowZero = false;
        }
    }
}
