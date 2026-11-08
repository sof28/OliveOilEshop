using ModularCommerce.Abstractions;
using ModularCommerce.Constants;
using ModularCommerce.Extensions;

namespace ModularCommerce.Features.ProductFeature;

public class AddProductEndpoint : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("products", async (
            IHandler<AddProductRequest, Result<AddProductResponse>> handler,
            AddProductRequest request,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(request, cancellationToken);
            return result.Match(
                onSuccess: response => Results.Created($"/products/{response.Id}", response),
                onFailure: error => Results.BadRequest(error)
            );
        })
        .WithTags(ApiTags.Products)
        .Produces<AddProductResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status400BadRequest);
    }
}

