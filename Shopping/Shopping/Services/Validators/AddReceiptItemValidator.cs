using FluentValidation;
using Shopping.Mediator;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class AddReceiptItemValidator : Validator<AddReceiptItem>
    {
        public AddReceiptItemValidator()
        {
            RuleFor(m => m.ProductId)
                .NotEmpty().WithMessage("The ProductId is required.");
            RuleFor(m => m.Price)
                .GreaterThan(0).WithMessage("The Price can not be zero or negative.");
            RuleFor(m => m.Amount)
                .GreaterThan(0).WithMessage("The Amount can not be zero or negative.");
        }
    }
}