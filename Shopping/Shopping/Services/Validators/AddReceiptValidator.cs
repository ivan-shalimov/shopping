using FluentValidation;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class AddReceiptValidator : AbstractValidator<AddReceipt>
    {
        public AddReceiptValidator()
        {
            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("The description should not be empty.");
        }
    }
}