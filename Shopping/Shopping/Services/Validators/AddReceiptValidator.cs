using FluentValidation;
using Shopping.Mediator;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class AddReceiptValidator : Validator<AddReceipt>
    {
        public AddReceiptValidator()
        {
            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("The description should not be empty.");
        }
    }
}