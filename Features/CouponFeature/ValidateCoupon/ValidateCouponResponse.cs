namespace ModularCommerce.Features.CouponFeature.ValidateCoupon;

public sealed record ValidateCouponResponse(string Code, bool FreeShipping, decimal? DiscountPercent);
