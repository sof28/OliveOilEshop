using FluentValidation;
using ModularCommerce.Entities;
using ModularCommerce.Repository;

namespace ModularCommerce.Features.OrderFeature.UpdateOrder;

public class UpdateOrderValidator : AbstractValidator<UpdateOrderRequest>
{
    public UpdateOrderValidator(IRepository<User> userRepo)
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Order ID must be greater than zero.");
        RuleFor(x => x.OrderItems)
            .NotEmpty().WithMessage("Order must contain at least one item.");
        RuleFor(x => x.CustomerAddress)
            .NotEmpty().WithMessage("Customer Address is required.")
            .MaximumLength(100).WithMessage("Customer Address must not exceed 100 characters");
        RuleFor(x => x.CustomerName)
           .NotEmpty().WithMessage("Customer Name is required.")
           .MaximumLength(100).WithMessage("Customer Name must not exceed 100 characters");
        RuleFor(x => x.CustomerEmail)
            .NotEmpty().WithMessage("Customer Email is required.")
            .EmailAddress().WithMessage("Customer Email must be a valid email address.");
        RuleFor(x => x.Price)
           .GreaterThan(0).WithMessage("Total Price must be greater than zero.");
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

