using FluentValidation;

namespace ModularCommerce.Features.ContactFeature.SubmitContact;

public class SubmitContactValidator : AbstractValidator<SubmitContactRequest>
{
    public SubmitContactValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(200);
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Email must be a valid email address.");
        RuleFor(x => x.Phone)
            .MaximumLength(50);
        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.")
            .MaximumLength(4000);
        RuleFor(x => x.ConsentGiven)
            .Equal(true).WithMessage("Consent is required.");
    }
}
