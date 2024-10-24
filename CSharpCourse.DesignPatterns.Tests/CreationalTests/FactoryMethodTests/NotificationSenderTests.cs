using CSharpCourse.DesignPatterns.Creational.FactoryMethod;
using CSharpCourse.DesignPatterns.Tests.Utils;

namespace CSharpCourse.DesignPatterns.Tests.CreationalTests.FactoryMethodTests;

public class NotificationSenderTests
{
    [Fact]
    public void EmailNotification()
    {
        var sender = new EmailNotificationSender();
        var output = OutputUtils.CaptureConsoleOutput(
            () => sender.SendNotification("Test email message"));

        Assert.Contains("Sending email notification: Test email message", output);
    }

    [Fact]
    public void SmsNotification()
    {
        var sender = new SmsNotificationSender();
        var output = OutputUtils.CaptureConsoleOutput(
            () => sender.SendNotification("Test SMS message"));

        Assert.Contains("Sending SMS notification: Test SMS message", output);
    }

    [Fact]
    public void PushNotification()
    {
        var sender = new PushNotificationSender();
        var output = OutputUtils.CaptureConsoleOutput(
            () => sender.SendNotification("Test push message"));

        Assert.Contains("Sending push notification: Test push message", output);
    }

    [Fact]
    public void TestFactoryMethodFlexibility()
    {
        NotificationSender[] senders =
        [
            new EmailNotificationSender(),
            new SmsNotificationSender(),
            new PushNotificationSender()
        ];
        string message = "Test message";

        // Act & Assert
        foreach (var sender in senders)
        {
            var output = OutputUtils.CaptureConsoleOutput(
                () => sender.SendNotification(message));
            Assert.Contains("Sending", output);
            Assert.Contains(message, output);
        }
    }
}
