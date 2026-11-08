using ModularCommerce.Abstractions;
using ModularCommerce.Entities;
using ModularCommerce.Repository;

namespace ModularCommerce.Features.BlogFeature.GetBlogPost;

public sealed class GetBlogPostHandler(IRepository<BlogPost> blogRepository)
    : IHandler<GetBlogPostQuery, Result<BlogPostResponse>>
{
    public async Task<Result<BlogPostResponse>> HandleAsync(
        GetBlogPostQuery query, CancellationToken cancellationToken)
    {
        var matches = (await blogRepository.GetAllAsync(cancellationToken))
            .Where(p => p.IsPublished &&
                        string.Equals(p.Slug, query.Slug, StringComparison.OrdinalIgnoreCase))
            .ToList();

        var post = matches.FirstOrDefault(p =>
                       string.Equals(p.Language, query.Language, StringComparison.OrdinalIgnoreCase))
                   ?? matches.FirstOrDefault();

        return post is null
            ? Result.Failure<BlogPostResponse>(BlogErrors.NotFound(query.Slug))
            : Result.Success(BlogMapping.ToResponse(post));
    }
}
