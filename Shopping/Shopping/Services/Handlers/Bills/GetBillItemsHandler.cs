using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Handlers.Bills
{
    public sealed class GetBillItemsHandler : IRequestHandler<GetBillItems, Either<Fail, BillItemModel[]>>
    {
        private readonly ShoppingDbContext _context;

        public GetBillItemsHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, BillItemModel[]>> Handle(GetBillItems request, CancellationToken cancellationToken)
        {
            var query = from billItem in _context.BillItems
                        join tariff in _context.Tariffs on billItem.TariffId equals tariff.Id
                        where billItem.BillId == request.BillId
                        select new BillItemModel
                        {
                            Id = billItem.Id,
                            BillId = billItem.BillId,
                            PreviousValue = billItem.PreviousValue,
                            Quantity = billItem.Quantity,
                            Rate = billItem.Rate,

                            Group = tariff.GroupName,
                            Description = tariff.Description,
                            Quantifiable = tariff.Quantifiable,
                            IsReadonlyRate = tariff.RateType == Models.Domain.RateType.Constant,
                        };
            var result = await query.ToArrayAsync();
            return result;
        }
    }
}