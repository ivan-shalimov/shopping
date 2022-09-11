using FluentValidation;
using Shopping.DataAccess;
using Shopping.Shared.Requests;

namespace Shopping.Services.Validators
{
    public sealed class DeleteProductValidator : AbstractValidator<DeleteProduct>
    {
        public DeleteProductValidator(ShoppingDbContext context)
        {
            RuleFor(m => m.Id)
                .CustomAsync(async (productId, ctx, cnt) =>
                {
                    var product = await context.Products.FindAsync(new object[] { productId }, cnt);
                    if (product == null)
                    {
                        ctx.AddFailure("Product is not found!");
                    }
                });
        }
    }
}