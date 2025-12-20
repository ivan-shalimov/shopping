using FluentValidation;
using Shopping.Mediator;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Validators
{
    public sealed class DeleteBillValidator : Validator<DeleteBill>
    {
        public DeleteBillValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}