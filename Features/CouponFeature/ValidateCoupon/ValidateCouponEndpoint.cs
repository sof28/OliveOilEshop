using ModularCommerce.Abstractions;
using ModularCommerce.Constants;
using ModularCommerce.Extensions;

namespace ModularCommerce.Features.CouponFeature.ValidateCoupon;

internal sealed class ValidateCouponEndpoint : IApiEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("coupons/validate", async (
            IHandler<ValidateCouponRequest, Result<ValidateCouponResponse>> handler,
            ValidateCouponRequest request,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(request, cancellationToken);
            return result.Match(
                onSuccess: response => Results.Ok(response),
                onFailure: error => Results.BadRequest(error)
            );
        })
        .WithTags(ApiTags.Coupons)
        .Produces<ValidateCouponResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .AllowAnonymous();
    }
}
