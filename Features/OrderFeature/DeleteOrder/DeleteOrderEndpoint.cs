using Microsoft.AspNetCore.Mvc;
using OliveOilEshop.Abstractions;
using OliveOilEshop.Constants;
using OliveOilEshop.Extensions;
using OliveOilEshop.Features.OrderFeature.CreateOrder;

namespace OliveOilEshop.Features.OrderFeature.DeleteOrder
{
    public class DeleteOrderEndpoint : IApiEndpoint
    {
        public void MapEndpoint(WebApplication app)
        {
            app.MapDelete("deleteOrder/{id}", async (IHandler<DeleteOrderRequest, Result<DeleteOrderResponse>> handler, [FromRoute] int id, CancellationToken cancellationToken) =>
            {
                var result = await handler.HandleAsync(new DeleteOrderRequest(id), cancellationToken);
                return result.Match(
                    onSuccess: response => Results.Ok(response),
                    onFailure: error => Results.BadRequest(error)
                );
            })
            .WithTags(ApiTags.Orders)
            .Produces<DeleteOrderResponse>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest);
        }
    }
}
