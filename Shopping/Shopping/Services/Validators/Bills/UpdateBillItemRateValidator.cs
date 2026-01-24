using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Validators
{
    public sealed class UpdateBillItemRateValidator : AbstractValidator<UpdateBillItemRate>
    {
        public UpdateBillItemRateValidator(ShoppingDbContext context)
        {
            RuleFor(m => m.Id)
                .NotEmpty().WithMessage("Id is required.");
            RuleFor(m => m.BillId)
                .NotEmpty().WithMessage("BillId is required.");
            RuleFor(m => m.Rate)
                .GreaterThanOrEqualTo(0).WithMessage("The Rate can not be negative.");

            RuleFor(m => m)
                .CustomAsync(async (r, ctx, cnt) =>
                {
                    var billItem = await context.BillItems
                        .FindAsync(new object[] { r.Id }, cnt)
                        .ConfigureAwait(false);
                    if (billItem == null)
                    {
                        ctx.AddFailure("Bill item not found for the provided Id and BillId.");
                        return;
                    }

                    var tariff = await context.Tariffs
                        .FirstOrDefaultAsync(t => t.Id == billItem.TariffId, cnt)
                        .ConfigureAwait(false);
                    if (tariff == null)
                    {
                        ctx.AddFailure("Tariff not found for the provided bill item.");
                        return;
                    }

                    if (tariff.RateType != Models.Domain.RateType.Variable)
                    {
                        ctx.AddFailure("The Rate can be updated only for the Variable rate type.");
                    }
                });
        }
    }
}