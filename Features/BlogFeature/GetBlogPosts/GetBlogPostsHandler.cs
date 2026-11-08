using ModularCommerce.Abstractions;
using ModularCommerce.Entities;
using ModularCommerce.Repository;

namespace ModularCommerce.Features.BlogFeature.GetBlogPosts;

public sealed class GetBlogPostsHandler(IRepository<BlogPost> blogRepository)
    : IHandler<GetBlogPostsQuery, Result<List<BlogPostResponse>>>
{
    public async Task<Result<List<BlogPostResponse>>> HandleAsync(
        GetBlogPostsQuery query, CancellationToken cancellationToken)
    {
        var posts = (await blogRepository.GetAllAsync(cancellationToken))
            .Where(p => p.IsPublished)
            .Where(p => string.IsNullOrWhiteSpace(query.Language) ||
                        string.Equals(p.Language, query.Language, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(p => p.PublishedAt)
            .Select(BlogMapping.ToResponse)
            .ToList();

        return Result.Success(posts);
    }
}
