using FluentValidation;
using Shopping.Mediator;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Validators.Bills
{
    public sealed class CreateBillValidator : Validator<CreateBill>
    {
        public CreateBillValidator()
        {
            RuleFor(m => m.Year)
                .GreaterThan(2020).WithMessage("Year can not be less 2020.")
                .LessThanOrEqualTo(DateTime.Now.Year).WithMessage("Year can not be less 2020.");
            RuleFor(m => m.Month)
                .GreaterThan(0).WithMessage("The Month can not be zero or negative.")
                .LessThanOrEqualTo(12).WithMessage("The Month can not be greater than 12.");
        }
    }
}