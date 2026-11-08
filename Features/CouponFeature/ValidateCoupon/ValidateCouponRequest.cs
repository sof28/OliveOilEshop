namespace ModularCommerce.Features.CouponFeature.ValidateCoupon;

public sealed record ValidateCouponRequest(string Code, decimal OrderAmount);
