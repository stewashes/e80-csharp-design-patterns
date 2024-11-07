using CSharpCourse.DesignPatterns.Behavioral.Mediator;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace CSharpCourse.DesignPatterns.Tests.BehavioralTests.MediatorTests;

public class BlogPostMediatorTests
{
    [Fact]
    public async Task CreateBlogPostCommand_Should_CreateBlogPost()
    {
        var mockRepository = new Mock<IBlogPostRepository>();
        mockRepository.Setup(repo => repo.AddAsync(It.IsAny<BlogPost>()))
                      .Returns(Task.CompletedTask);

        var services = new ServiceCollection();
        services.AddSingleton(mockRepository.Object);
        services.AddMediatR(
            c => c.RegisterServicesFromAssemblyContaining<BlogPost>());

        var serviceProvider = services.BuildServiceProvider();
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var command = new CreateBlogPostCommand
        {
            Title = "Test Title",
            Content = "Test Content"
        };

        var result = await mediator.Send(command);

        Assert.NotNull(result);
        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.Content, result.Content);
        mockRepository.Verify(repo => repo.AddAsync(It.IsAny<BlogPost>()), Times.Once);

        // If needed, we can mock request handlers and re-register them in
        // the service collection to override the ones found in the assembly.
    }
}
