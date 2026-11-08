namespace ModularCommerce.Features.ProductFeature;

public sealed record AddProductRequest(
    string Name,
    string? Description,
    decimal Price,
    string? ImageUrl,
    string? Sku,
    int StockQuantity,
    long? CategoryId,
    long? ProductTypeId);

