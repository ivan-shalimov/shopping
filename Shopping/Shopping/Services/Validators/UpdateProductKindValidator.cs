using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class UpdateProductKindValidator : AbstractValidator<UpdateProductKind>
    {
        public UpdateProductKindValidator(ShoppingDbContext context)
        {
            RuleFor(m => m)
                .CustomAsync(async (model, ctx, cnt) =>
                {
                    var isNameUsed = await context.ProductKinds.AnyAsync(p => p.Name == model.Name && p.Id != model.Id, cnt);
                    if (isNameUsed)
                    {
                        ctx.AddFailure($"The product kind name is already used.");
                    }
                });
        }
    }
}