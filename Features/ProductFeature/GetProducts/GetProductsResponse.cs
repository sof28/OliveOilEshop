namespace OliveOilEshop.Features.ProductFeature.GetProducts;

public sealed record GetProductsResponse(long Id, string Name, string? Description, decimal Price, string? ImageUrl);

