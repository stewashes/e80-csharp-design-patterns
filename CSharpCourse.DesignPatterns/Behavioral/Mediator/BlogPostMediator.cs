using MediatR;

namespace CSharpCourse.DesignPatterns.Behavioral.Mediator;

// Domain model
internal class BlogPost
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public required string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

// Repository interface
internal interface IBlogPostRepository
{
    Task<BlogPost> GetByIdAsync(int id);
    Task AddAsync(BlogPost blogPost);
    Task UpdateAsync(BlogPost blogPost);
    Task DeleteByIdAsync(int id);
}

#region Commands (CQRS)
// Commands mutate the state of the system
internal record CreateBlogPostCommand : IRequest<BlogPost>
{
    public required string Title { get; set; }
    public required string Content { get; set; }
}

internal record UpdateBlogPostCommand : IRequest<BlogPost>
{
    public int Id { get; set; }
    public required string Title { get; set; }
}

internal record DeleteBlogPostCommand : IRequest<Unit> // returns void
{
    public int Id { get; set; }
}
#endregion

#region Queries (CQRS)
// Queries return (projections of) data from the system, they don't mutate the state.
internal record GetBlogPostQuery : IRequest<BlogPost>
{
    public int Id { get; set; }
}
#endregion

public class BlogPostException : Exception
{
    public BlogPostException(string message) : base(message)
    {
    }
}

// MediatR scans the assembly for IRequestHandler implementations,
// runs some reflection, and then generates runtime types to handle the requests.
// There is another library called Mediator that uses C# 9 source generators to
// generate everything at compile time. It has the same API as MediatR.
// https://github.com/martinothamar/Mediator

// Each request has its own handler which returns a given response.
// The user simply pushes the request, and the mediator finds the right handler to process it.
// The user doesn't need to know about the handler, just the request and the response.

// Request Handlers
internal class CreateBlogPostHandler : IRequestHandler<CreateBlogPostCommand, BlogPost>
{
    private readonly IBlogPostRepository _repository;

    public CreateBlogPostHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogPost> Handle(CreateBlogPostCommand request, CancellationToken cancellationToken)
    {
        var blogPost = new BlogPost
        {
            Title = request.Title,
            Content = request.Content,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(blogPost);
        return blogPost;
    }
}

internal class UpdateBlogPostHandler : IRequestHandler<UpdateBlogPostCommand, BlogPost>
{
    private readonly IBlogPostRepository _repository;

    public UpdateBlogPostHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogPost> Handle(UpdateBlogPostCommand request, CancellationToken cancellationToken)
    {
        var blogPost = await _repository.GetByIdAsync(request.Id)
            ?? throw new BlogPostException("Blog post not found");

        blogPost.Title = request.Title;
        blogPost.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(blogPost);
        return blogPost;
    }
}

internal class DeleteBlogPostHandler : IRequestHandler<DeleteBlogPostCommand, Unit>
{
    private readonly IBlogPostRepository _repository;

    public DeleteBlogPostHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteByIdAsync(request.Id);
        return Unit.Value;
    }
}

internal class GetBlogPostHandler : IRequestHandler<GetBlogPostQuery, BlogPost>
{
    private readonly IBlogPostRepository _repository;

    public GetBlogPostHandler(IBlogPostRepository repository)
    {
        _repository = repository;
    }

    public async Task<BlogPost> Handle(GetBlogPostQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id);
    }
}
