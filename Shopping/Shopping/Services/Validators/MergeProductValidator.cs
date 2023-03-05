using FluentValidation;
using Shopping.DataAccess;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class MergeProductValidator : AbstractValidator<MergeProduct>
    {
        public MergeProductValidator(ShoppingDbContext context)
        {
            RuleFor(m => m)
                .CustomAsync(async (model, ctx, cnt) =>
                {
                    var firstProduct = await context.Products.FindAsync(new object[] { model.SavedProductId }, cnt);
                    if (firstProduct == null)
                    {
                        ctx.AddFailure("Saved product is not found!");
                        return;
                    }

                    var secondProduct = await context.Products.FindAsync(new object[] { model.RemovedProductId }, cnt);
                    if (secondProduct == null)
                    {
                        ctx.AddFailure("Removed product is not found!");
                        return;
                    }
                });
        }
    }
}