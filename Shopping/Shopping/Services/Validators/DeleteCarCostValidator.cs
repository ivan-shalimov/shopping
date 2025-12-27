using FluentValidation;
using Shopping.DataAccess;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class DeleteCarCostValidator : AbstractValidator<DeleteCarCost>
    {
        public DeleteCarCostValidator(ShoppingDbContext context)
        {
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