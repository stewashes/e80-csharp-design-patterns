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
