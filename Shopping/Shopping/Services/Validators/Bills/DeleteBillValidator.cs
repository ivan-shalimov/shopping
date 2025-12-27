using FluentValidation;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Validators
{
    public sealed class DeleteBillValidator : AbstractValidator<DeleteBill>
    {
        public DeleteBillValidator()
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}