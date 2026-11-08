using ModularCommerce.Abstractions;
using ModularCommerce.Entities;
using ModularCommerce.Repository;

namespace ModularCommerce.Features.CouponFeature.ValidateCoupon;

public sealed class ValidateCouponHandler(IRepository<Coupon> couponRepository)
    : IHandler<ValidateCouponRequest, Result<ValidateCouponResponse>>
{
    public async Task<Result<ValidateCouponResponse>> HandleAsync(
        ValidateCouponRequest command, CancellationToken cancellationToken)
    {
        var code = command.Code.Trim();
        var coupon = (await couponRepository.GetAllAsync(cancellationToken))
            .FirstOrDefault(c => c.IsActive &&
                                 string.Equals(c.Code, code, StringComparison.OrdinalIgnoreCase) &&
                                 (c.ValidUntil is null || c.ValidUntil >= DateTime.UtcNow));

        if (coupon is null)
        {
            return Result.Failure<ValidateCouponResponse>(CouponErrors.Invalid(code));
        }

        if (command.OrderAmount < coupon.MinOrderAmount)
        {
            return Result.Failure<ValidateCouponResponse>(CouponErrors.MinimumNotReached(coupon.MinOrderAmount));
        }

        return Result.Success(new ValidateCouponResponse(coupon.Code, coupon.FreeShipping, coupon.DiscountPercent));
    }
}
