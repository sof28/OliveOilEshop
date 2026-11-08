namespace ModularCommerce.Features.BlogFeature;

public sealed record BlogPostResponse(
    long Id,
    string Slug,
    string Title,
    string? Excerpt,
    string Content,
    string? ImageUrl,
    string Language,
    DateTime PublishedAt);
