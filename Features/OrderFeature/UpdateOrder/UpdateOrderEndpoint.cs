using Microsoft.AspNetCore.Authorization;
using ModularCommerce.Abstractions;
using ModularCommerce.Constants;
using ModularCommerce.Extensions;

namespace ModularCommerce.Features.OrderFeature.UpdateOrder;

internal sealed class UpdateOrderEndpoint : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("order/{id}", [Authorize] async (IHandler<UpdateOrderRequest, Result<UpdateOrderResponse>> handler, long id, UpdateOrderRequest command, CancellationToken cancellationToken) =>
        {
            var updateCommand = command with { Id = id };
            var result = await handler.HandleAsync(command, cancellationToken);
            return result.Match(
                onSuccess: response => Results.Ok(response),
                onFailure: error => Results.BadRequest(error)
            );
        })
        .WithTags(ApiTags.Orders)
        .Produces<UpdateOrderResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}

