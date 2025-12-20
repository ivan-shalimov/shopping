using FluentValidation;
using Shopping.Mediator;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class AddCarCostValidator : Validator<AddCarCost>
    {
        public AddCarCostValidator()
        {
            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("Description is required.");
            RuleFor(m => m.Price)
                .GreaterThan(0).WithMessage("The Price can not be zero or negative.");
            RuleFor(m => m.Amount)
                .GreaterThan(0).WithMessage("The Amount can not be zero or negative.");
        }
    }
}