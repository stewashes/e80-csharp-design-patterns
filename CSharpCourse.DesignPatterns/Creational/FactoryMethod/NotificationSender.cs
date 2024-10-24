namespace CSharpCourse.DesignPatterns.Creational.FactoryMethod;

// Abstract product
internal interface INotification
{
    void Send(string message);
}

// Concrete products
internal class EmailNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending email notification: {message}");
    }
}

internal class SmsNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending SMS notification: {message}");
    }
}

internal class PushNotification : INotification
{
    public void Send(string message)
    {
        Console.WriteLine($"Sending push notification: {message}");
    }
}

// Abstract creator
internal abstract class NotificationSender
{
    // Factory Method
    public abstract INotification CreateNotification();

    // Template method that uses the factory method
    public void SendNotification(string message)
    {
        INotification notification = CreateNotification();
        notification.Send(message);
    }
}

// Concrete creators
internal class EmailNotificationSender : NotificationSender
{
    public override INotification CreateNotification()
    {
        return new EmailNotification();
    }
}

internal class SmsNotificationSender : NotificationSender
{
    public override INotification CreateNotification()
    {
        return new SmsNotification();
    }
}

internal class PushNotificationSender : NotificationSender
{
    public override INotification CreateNotification()
    {
        return new PushNotification();
    }
}
