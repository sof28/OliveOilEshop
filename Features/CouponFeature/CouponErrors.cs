using ModularCommerce.Abstractions.Errors;

namespace ModularCommerce.Features.CouponFeature;

public static class CouponErrors
{
    public static Error Invalid(string code) =>
        Error.NotFound("Coupon.Invalid", $"The coupon '{code}' is not valid");

    public static Error MinimumNotReached(decimal minimum) =>
        Error.Validation("Coupon.MinimumNotReached", $"The order amount must be at least {minimum:0.00}");
}
