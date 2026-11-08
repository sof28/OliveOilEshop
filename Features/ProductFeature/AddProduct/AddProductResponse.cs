namespace ModularCommerce.Features.ProductFeature;

public sealed record AddProductResponse(
    long Id,
    string Name,
    string Slug,
    string? Description,
    decimal Price,
    string? ImageUrl,
    string? Sku,
    int StockQuantity,
    long? CategoryId,
    long? ProductTypeId);

