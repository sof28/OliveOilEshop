using ModularCommerce.Abstractions;
using ModularCommerce.Constants;
using ModularCommerce.Extensions;

namespace ModularCommerce.Features.BlogFeature.GetBlogPosts;

internal sealed class GetBlogPostsEndpoint : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("blog", async (
            IHandler<GetBlogPostsQuery, Result<List<BlogPostResponse>>> handler,
            string? lang,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetBlogPostsQuery(lang), cancellationToken);
            return result.Match(
                onSuccess: response => Results.Ok(response),
                onFailure: error => Results.BadRequest(error)
            );
        })
        .WithTags(ApiTags.Blog)
        .Produces<List<BlogPostResponse>>(StatusCodes.Status200OK)
        .AllowAnonymous();
    }
}
