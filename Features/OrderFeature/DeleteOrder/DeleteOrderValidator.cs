using FluentValidation;

namespace ModularCommerce.Features.OrderFeature.DeleteOrder
{
    public class DeleteOrderValidator : AbstractValidator<DeleteOrderRequest>
    {
        public DeleteOrderValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Order ID must be greater than 0.");
        }
    }
}

