using FluentValidation;
using Shopping.DataAccess;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class UpdateCarCostValidator : AbstractValidator<UpdateCarCost>
    {
        public UpdateCarCostValidator(ShoppingDbContext context)
        {
            RuleFor(m => m.Description)
                .NotEmpty().WithMessage("Description is required.");
            RuleFor(m => m.Price)
                .GreaterThan(0).WithMessage("The Price can not be zero or negative.");
            RuleFor(m => m.Amount)
                .GreaterThan(0).WithMessage("The Amount can not be zero or negative.");
            RuleFor(m => m.Id)
                .CustomAsync(async (carCostId, ctx, cnt) =>
                {
                    var carCost = await context.CarCosts.FindAsync(new object[] { carCostId }, cnt);
                    if (carCost == null)
                    {
                        ctx.AddFailure("Car cost is not found!");
                        return;
                    }
                });
        }
    }
}