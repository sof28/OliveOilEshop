using Microsoft.AspNetCore.Authorization;
using OliveOilEshop.Abstractions;
using OliveOilEshop.Constants;
using OliveOilEshop.Extensions;

namespace OliveOilEshop.Features.OrderFeature.CreateOrder;

internal sealed class CreateOrderEndpoint : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("orders", [Authorize] async (IHandler<CreateOrderRequest, Result<CreateOrderResponse>> handler, CreateOrderRequest command, CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(command, cancellationToken);
            return result.Match(
                onSuccess: response => Results.Ok(response),
                onFailure: error => Results.BadRequest(error)
            );
        })
        .WithTags(ApiTags.Orders)
        .Produces<CreateOrderResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest);
    }
}
