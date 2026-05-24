using OliveOilEshop.Abstractions;
using OliveOilEshop.Entities;
using OliveOilEshop.Repository;

namespace OliveOilEshop.Features.ProductFeature;

public sealed class AddProductHandler(
    IRepository<Product> productRepository,
    IUnitOfWork unitOfWork) : IHandler<AddProductRequest, Result<AddProductResponse>>
{
    public async Task<Result<AddProductResponse>> HandleAsync(
        AddProductRequest command, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            ImageUrl = command.ImageUrl
        };

        await productRepository.AddAsync(product, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        return Result.Success(new AddProductResponse(
            product.Id, product.Name, product.Description, product.Price, product.ImageUrl));
    }
}