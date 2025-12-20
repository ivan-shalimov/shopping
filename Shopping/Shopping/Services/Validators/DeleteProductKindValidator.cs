using FluentValidation;
using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class DeleteProductKindValidator : Validator<DeleteProductKind>
    {
        public DeleteProductKindValidator(ShoppingDbContext context)
        {
            RuleFor(m => m.Id)
                .CustomAsync(async (productKindId, ctx, cnt) =>
                {
                    var productKind = await context.ProductKinds.FindAsync(new object[] { productKindId }, cnt);
                    if (productKind == null)
                    {
                        ctx.AddFailure("Product kind is not found!");
                        return;
                    }

                    var hasAnyProducts = await context.Products.AnyAsync(p => p.ProductKindId == productKindId, cnt);
                    if (hasAnyProducts)
                    {
                        ctx.AddFailure($"The product kind has products.");
                    }
                });
        }
    }
}