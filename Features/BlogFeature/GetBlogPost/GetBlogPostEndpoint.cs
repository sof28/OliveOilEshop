using ModularCommerce.Abstractions;
using ModularCommerce.Abstractions.Errors;
using ModularCommerce.Constants;
using ModularCommerce.Extensions;

namespace ModularCommerce.Features.BlogFeature.GetBlogPost;

internal sealed class GetBlogPostEndpoint : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("blog/{slug}", async (
            IHandler<GetBlogPostQuery, Result<BlogPostResponse>> handler,
            string slug,
            string? lang,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetBlogPostQuery(slug, lang), cancellationToken);
            return result.Match(
                onSuccess: response => Results.Ok(response),
                onFailure: error => error.Type == ErrorType.NotFound
                    ? Results.NotFound(error)
                    : Results.BadRequest(error)
            );
        })
        .WithTags(ApiTags.Blog)
        .Produces<BlogPostResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .AllowAnonymous();
    }
}
