using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Mediator;
using Shopping.Models.Requests;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;

namespace Shopping.Services.Handlers.Bills
{
    public sealed class UpdateBillTotalHandler : IRequestHandler<UpdateBillTotal, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public UpdateBillTotalHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(UpdateBillTotal request, CancellationToken cancellationToken)
        {
            var bill = await _context.Bills.FindAsync([request.Id], cancellationToken);
            if (bill == null)
            {
                return new Fail(FailType.Validation, ["Bill item is not found"]);
            }

            var query = from billItem in _context.BillItems
                        join tariff in _context.Tariffs on billItem.TariffId equals tariff.Id
                        where billItem.BillId == request.Id
                        select new BillItemModel
                        {
                            Id = billItem.Id,
                            BillId = billItem.BillId,
                            Quantity = billItem.Quantity,
                            Quantifiable = tariff.Quantifiable,
                            Rate = billItem.Rate,
                        };
            var billItems = await query.ToArrayAsync(cancellationToken);

            var total = billItems.Sum(item => item.Quantifiable ? item.Quantity * item.Rate : item.Rate);
            bill.Total = (int)(100 * total); // calculate total in minor units
            await _context.SaveChangesAsync();

            return Success.Instance;
        }
    }
}