using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CSharpCourse.DesignPatterns.Creational.Builder;

// The Options Configuration pattern is a way to configure options for a service.
// It is a variation of the Builder pattern, where the options are built by the DI container.
// This pattern is extensively used in ASP.NET Core for configuring services.

// Configuration class for email service
internal class EmailOptions
{
    public string? SmtpServer { get; set; }
    public int Port { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public bool UseSsl { get; set; }
}

internal interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    EmailOptions CurrentOptions { get; }
}

internal class EmailService : IEmailService
{
    private readonly EmailOptions _options;

    // We make use of IOptions<T> to get the configured options
    public EmailService(IOptions<EmailOptions> options)
    {
        _options = options.Value ?? throw new ArgumentNullException(nameof(options));
    }

    public EmailOptions CurrentOptions => _options;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        if (string.IsNullOrEmpty(_options.SmtpServer))
        {
            throw new InvalidOperationException("SMTP server is not configured");
        }

        Console.WriteLine($"Sending email via {_options.SmtpServer}:{_options.Port}");

        // Simulate sending an email
        await Task.Delay(100);
    }
}

// Extension method to add the email service to the DI container
internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEmailService(
        this IServiceCollection services,
        Action<EmailOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddScoped<IEmailService, EmailService>();
        return services;
    }
}
