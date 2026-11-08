using ModularCommerce.Abstractions;
using ModularCommerce.Constants;
using ModularCommerce.Extensions;

namespace ModularCommerce.Features.ProductFeature.GetProducts;

internal sealed class GetProductsEndpoint : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("products", async (
            IHandler<GetProductsQuery, Result<List<GetProductsResponse>>> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetProductsQuery(), cancellationToken);
            return result.Match(
                onSuccess: response => Results.Ok(response),
                onFailure: error => Results.BadRequest(error)
            );
        })
        .WithTags(ApiTags.Products)
        .Produces<List<GetProductsResponse>>(StatusCodes.Status200OK)
        .AllowAnonymous();
    }
}


