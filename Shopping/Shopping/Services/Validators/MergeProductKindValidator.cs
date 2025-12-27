using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class MergeProductKindValidator : AbstractValidator<MergeProductKind>
    {
        public MergeProductKindValidator(ShoppingDbContext context)
        {
            RuleFor(m => m)
                .CustomAsync(async (model, ctx, cnt) =>
                {
                    var firstProductKind = await context.ProductKinds.FindAsync(new object[] { model.FirstProductKindId }, cnt);
                    if (firstProductKind == null)
                    {
                        ctx.AddFailure("First product kind is not found!");
                        return;
                    }

                    var secondProductKind = await context.ProductKinds.FindAsync(new object[] { model.SecondProductKindId }, cnt);
                    if (secondProductKind == null)
                    {
                        ctx.AddFailure("Second product kind is not found!");
                        return;
                    }

                    var isNameUsed = await context.ProductKinds.AnyAsync(p => p.Name == model.NewProductKindName 
                                                                     && (p.Id != model.FirstProductKindId || p.Id == model.SecondProductKindId), cnt);
                    if (isNameUsed)
                    {
                        ctx.AddFailure($"The product kind {model.NewProductKindName} already exists.");
                    }
                });
        }
    }
}