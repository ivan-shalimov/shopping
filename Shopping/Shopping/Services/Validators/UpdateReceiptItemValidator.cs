using FluentValidation;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class UpdateReceiptItemValidator : AbstractValidator<UpdateReceiptItem>
    {
        public UpdateReceiptItemValidator()
        {
            RuleFor(m => m.Price)
                .GreaterThan(0).WithMessage("The Price can not be zero or negative.");
            RuleFor(m => m.Amount)
                .GreaterThan(0).WithMessage("The Amount can not be zero or negative.");
        }
    }
}