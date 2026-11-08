using ModularCommerce.Abstractions;
using ModularCommerce.Entities;
using ModularCommerce.Repository;

namespace ModularCommerce.Features.ProductFeature.GetProducts;

public sealed class GetProductsHandler(IRepository<Product> productRepository)
    : IHandler<GetProductsQuery, Result<List<GetProductsResponse>>>
{
    public async Task<Result<List<GetProductsResponse>>> HandleAsync(
        GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllAsync(cancellationToken);
        var response = products
            .Cast<Product>()
            .Select(p => new GetProductsResponse(
                p.Id,
                p.Name,
                p.Slug,
                p.Description,
                p.Price,
                p.ImageUrl,
                p.Sku,
                p.StockQuantity,
                p.CategoryId,
                p.Category?.Name,
                p.ProductTypeId,
                p.ProductType?.Name))
            .ToList();
        return Result.Success(response);
    }
}

