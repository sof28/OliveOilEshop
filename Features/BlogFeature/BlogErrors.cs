using ModularCommerce.Abstractions.Errors;

namespace ModularCommerce.Features.BlogFeature;

public static class BlogErrors
{
    public static Error NotFound(string slug) =>
        Error.NotFound("Blog.NotFound", $"The blog post '{slug}' was not found");
}
