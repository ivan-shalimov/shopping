using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class AddProductKindValidator : AbstractValidator<AddProductKind>
    {
        public AddProductKindValidator(ShoppingDbContext context)
        {
            RuleFor(m => m.Name)
                .CustomAsync(async (productKindName, ctx, cnt) =>
                {
                    var isNameUsed = await context.ProductKinds.AnyAsync(p => p.Name == productKindName, cnt);
                    if (isNameUsed)
                    {
                        ctx.AddFailure($"The product kind with this name is already exists.");
                    }
                });
        }
    }
}