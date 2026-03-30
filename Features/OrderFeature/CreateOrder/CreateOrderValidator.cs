using FluentValidation;
using OliveOilEshop.Entities;
using OliveOilEshop.Repository;

namespace OliveOilEshop.Features.OrderFeature.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderRequest>
    {
        public CreateOrderValidator(IRepository<User> userRepo)
        {
            RuleFor(x => x.OrderItems)
                .NotEmpty().WithMessage("Order must contain at least one item.");
            RuleFor(x => x.CustomerAddress)
                .NotEmpty().WithMessage("Customer address is required.");
            RuleFor(x => x.CustomerEmail)
                .NotEmpty().WithMessage("Customer email is required.")
                .EmailAddress().WithMessage("Customer email must be a valid email address.");
            RuleFor(x => x.Price)
               .GreaterThan(0).WithMessage("Total price must be greater than zero.");
            RuleFor(x => x.UserId)
               .GreaterThan(0).WithMessage("User ID must be greater than zero.");
            //TODO: Add validation for OrderItems (e.g., check if product exists, quantity > 0, etc.)
            //RuleForEach(x => x.OrderItems).SetValidator(new OrderItemValidator());

            // Only validate UserId if it has a value
            RuleFor(x => x.UserId)
               .GreaterThan(0).WithMessage("User ID must be greater than zero.")
               .When(x => x.UserId.HasValue)
               .MustAsync(async (userId, ct) => await OrderValidationHelpers.UserExistsAsync(userId: userId!.Value, userRepo, ct))
               .WithMessage("User does not exist.");
        }
    }
}
