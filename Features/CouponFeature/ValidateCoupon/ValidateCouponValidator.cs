using FluentValidation;

namespace ModularCommerce.Features.CouponFeature.ValidateCoupon;

public class ValidateCouponValidator : AbstractValidator<ValidateCouponRequest>
{
    public ValidateCouponValidator()
    {
        RuleFor(x => x.Code).NotEmpty().WithMessage("Coupon code is required.");
        RuleFor(x => x.OrderAmount).GreaterThanOrEqualTo(0);
    }
}
