using FluentValidation;
using Shopping.Mediator;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Validators
{
    public sealed class UpdateBillItemQuantityValidator : Validator<UpdateBillItemQuantity>
    {
        public UpdateBillItemQuantityValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id is required.");
            RuleFor(m => m.BillId)
                .NotEmpty().WithMessage("BillId is required.");
            RuleFor(m => m.Quantity)
                .GreaterThanOrEqualTo(0).WithMessage("The Quantity can not be negative.");
        }
    }
}