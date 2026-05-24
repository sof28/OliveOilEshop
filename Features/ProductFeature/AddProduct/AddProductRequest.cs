namespace OliveOilEshop.Features.ProductFeature;

public sealed record AddProductRequest(string Name, string? Description, decimal Price, string? ImageUrl);
