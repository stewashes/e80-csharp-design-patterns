using CSharpCourse.DesignPatterns.Creational.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpCourse.DesignPatterns.Tests.CreationalTests.BuilderTests;

public class EmailServiceTests
{
    [Fact]
    public void Authenticated()
    {
        var services = new ServiceCollection();

        services.AddEmailService(options =>
        {
            options.SmtpServer = "smtp.example.com";
            options.Port = 587;
            options.Username = "test@example.com";
            options.Password = "password123";
            options.UseSsl = true;
        });

        var serviceProvider = services.BuildServiceProvider();
        var emailService = serviceProvider.GetRequiredService<IEmailService>();

        var options = emailService.CurrentOptions;
        Assert.Equal("smtp.example.com", options.SmtpServer);
        Assert.Equal(587, options.Port);
        Assert.Equal("test@example.com", options.Username);
        Assert.Equal("password123", options.Password);
        Assert.True(options.UseSsl);
    }

    [Fact]
    public async Task Unauthenticated()
    {
        var services = new ServiceCollection();
        services.AddEmailService(options =>
        {
            // The options have defaults, so we don't need to set all
            // the properties in the test
            options.SmtpServer = "smtp.example.com";
            options.Port = 587;
        });

        var serviceProvider = services.BuildServiceProvider();
        var emailService = serviceProvider.GetRequiredService<IEmailService>();

        await emailService.SendEmailAsync("test@example.com", "Test", "Body");
        Assert.True(true); // suppresses xUnit warning
    }

    [Fact]
    public async Task NoServer()
    {
        var services = new ServiceCollection();
        services.AddEmailService(options => { });

        var serviceProvider = services.BuildServiceProvider();
        var emailService = serviceProvider.GetRequiredService<IEmailService>();

        await Assert.ThrowsAsync<InvalidOperationException>(
            () => emailService.SendEmailAsync("test@example.com", "Test", "Body")
        );
    }
}
