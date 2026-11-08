using ModularCommerce.Entities;

namespace ModularCommerce.Features.BlogFeature;

internal static class BlogMapping
{
    public static BlogPostResponse ToResponse(BlogPost p) => new(
        p.Id, p.Slug, p.Title, p.Excerpt, p.Content, p.ImageUrl, p.Language, p.PublishedAt);
}
