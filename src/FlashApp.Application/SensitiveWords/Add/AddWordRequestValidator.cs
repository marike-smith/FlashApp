using FluentValidation;

namespace FlashApp.Application.SensitiveWords.Add;
internal sealed class AddWordRequestValidator : AbstractValidator<AddWordResponse>
{
    public AddWordRequestValidator()
    {
        // RuleFor(c => c.FirstName)
        //         .NotEmpty()
        //         .MaximumLength(50);
        // RuleFor(c => c.LastName)
        //     .NotEmpty()
        //     .MaximumLength(50);
        // RuleFor(c => c.PhoneNumber)
        //     .NotEmpty()
        //     .MaximumLength(10).WithMessage("Please enter a valid number.")
        //     .Matches(@"^0\d{9}$").WithMessage("Please enter a valid number.");
        // RuleFor(c => c.EmailAddress.Value)
        //     .NotEmpty()
        //     .WithMessage("Please enter a valid email address.")
        //     .Matches(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$").WithMessage("Please enter a valid email address.");
        //     RuleFor(c => c.Password.Value)
        //     .NotEmpty();
    }
}
