using System.Threading.Channels;

namespace CSharpCourse.DesignPatterns.Behavioral.Observer;

// Pub/sub pattern. This can also be implemented in a microservices
// architecture through message brokers like RabbitMQ or Kafka.

internal readonly record struct Coordinates(
    Guid DeviceId,
    double Latitude,
    double Longitude);

internal class GpsService
{
    // Channel<T> is a thread-safe way to communicate between
    // producers and consumers in the pub/sub pattern.

    // Unbounded = no limit on the capacity.
    // Bounded = we can specify a maximum capacity, after which
    // the policy defined in FullMode will be applied.
    private readonly Channel<Coordinates> _channel = 
        Channel.CreateBounded<Coordinates>(new BoundedChannelOptions(10)
        {
            SingleWriter = true, // Single producer
            SingleReader = false, // Multiple consumers
            FullMode = BoundedChannelFullMode.DropOldest
        });

    public async Task PublishAsync(Coordinates coordinates)
        => await _channel.Writer.WriteAsync(coordinates);

    public IAsyncEnumerable<Coordinates> ReadCoordinatesAsync(CancellationToken cancellationToken = default)
        => _channel.Reader.ReadAllAsync(cancellationToken);

    public bool TryCompleteWriter(Exception? exception = null)
        => _channel.Writer.TryComplete(exception);
}
