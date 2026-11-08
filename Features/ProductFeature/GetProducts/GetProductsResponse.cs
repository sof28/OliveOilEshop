namespace ModularCommerce.Features.ProductFeature.GetProducts;

public sealed record GetProductsResponse(
    long Id,
    string Name,
    string Slug,
    string? Description,
    decimal Price,
    string? ImageUrl,
    string? Sku,
    int StockQuantity,
    long? CategoryId,
    string? CategoryName,
    long? ProductTypeId,
    string? ProductTypeName);

