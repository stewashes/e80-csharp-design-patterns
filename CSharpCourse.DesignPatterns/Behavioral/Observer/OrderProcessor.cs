namespace CSharpCourse.DesignPatterns.Behavioral.Observer;

internal class OrderProcessor
{
    // Events can have add/remove accessors to add more logic
    // when subscribing/unsubscribing to the event (e.g., if we wanted
    // to stop some periodic elaboration when no one is listening).

    // EventHandler<T> is just a wrapper for a delegate that takes
    // two parameters: the sender and the event arguments (T).
    public event EventHandler<OrderEventArgs>? OrderProcessed;
    public event Func<OrderEventArgs, Task>? OrderProcessedAsync;

    public void ProcessOrder(Guid orderId)
    {
        // Process order logic here

        OrderProcessed?.Invoke(this, new OrderEventArgs(orderId));
    }

    // CAREFUL: When using async events, the subject has no control
    // over how long the event handlers will take to execute! It's
    // better to return immediately and let the handlers schedule
    // their own long-running tasks on the thread pool, as to not block
    // the subject. Anyways, here's how one would implement such a thing.

    // Sequential
    public async Task ProcessOrderAsync(Guid orderId)
    {
        // Process order logic here

        if (OrderProcessedAsync != null)
        {
            // These are executed sequentially if there are
            // multiple subscribers.
            await OrderProcessedAsync(new OrderEventArgs(orderId));
        }
    }

    // Parallel
    public async Task ProcessOrderAsyncParallel(Guid orderId)
    {
        var args = new OrderEventArgs(orderId);
        var handlers = OrderProcessedAsync?.GetInvocationList();

        if (handlers != null)
        {
            var tasks = handlers
                .Cast<Func<OrderEventArgs, Task>>()
                .Select(handler => handler(args));

            await Task.WhenAll(tasks);
        }
    }
}

internal class OrderEventArgs(Guid orderId) : EventArgs
{
    public Guid OrderId { get; } = orderId;
}
