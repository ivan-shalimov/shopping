using FluentValidation;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class UpdateReceiptValidator : AbstractValidator<UpdateReceipt>
    {
        public UpdateReceiptValidator()
        {
            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("The description should not be empty.");
        }
    }
}