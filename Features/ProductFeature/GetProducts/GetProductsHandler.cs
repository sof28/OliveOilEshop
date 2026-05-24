using OliveOilEshop.Abstractions;
using OliveOilEshop.Entities;
using OliveOilEshop.Repository;

namespace OliveOilEshop.Features.ProductFeature.GetProducts;

public sealed class GetProductsHandler(IRepository<Product> productRepository)
    : IHandler<GetProductsQuery, Result<List<GetProductsResponse>>>
{
    public async Task<Result<List<GetProductsResponse>>> HandleAsync(
        GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllAsync(cancellationToken);
        var response = products
            .Select(p => new GetProductsResponse(p.Id, p.Name, p.Description, p.Price, p.ImageUrl))
            .ToList();
        return Result.Success(response);
    }
}

