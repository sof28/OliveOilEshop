using ModularCommerce.Abstractions;
using ModularCommerce.Entities;
using ModularCommerce.Repository;

namespace ModularCommerce.Features.ProductFeature;

public sealed class AddProductHandler(
    IRepository<Product> productRepository,
    IUnitOfWork unitOfWork) : IHandler<AddProductRequest, Result<AddProductResponse>>
{
    public async Task<Result<AddProductResponse>> HandleAsync(
        AddProductRequest command, CancellationToken cancellationToken)
    {
        var slug = command.Name.Trim().ToLowerInvariant().Replace(" ", "-");
        var product = new Product
        {
            Name = command.Name,
            Slug = slug,
            Description = command.Description,
            Price = command.Price,
            ImageUrl = command.ImageUrl,
            Sku = string.IsNullOrWhiteSpace(command.Sku) ? null : command.Sku.Trim(),
            StockQuantity = command.StockQuantity,
            CategoryId = command.CategoryId,
            ProductTypeId = command.ProductTypeId
        };

        await productRepository.AddAsync(product, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success(new AddProductResponse(
            product.Id,
            product.Name,
            product.Slug,
            product.Description,
            product.Price,
            product.ImageUrl,
            product.Sku,
            product.StockQuantity,
            product.CategoryId,
            product.ProductTypeId));
    }
}
