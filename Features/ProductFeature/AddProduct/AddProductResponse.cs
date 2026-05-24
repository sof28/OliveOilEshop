namespace OliveOilEshop.Features.ProductFeature;

public sealed record AddProductResponse(long Id, string Name, string? Description, decimal Price, string? ImageUrl);
