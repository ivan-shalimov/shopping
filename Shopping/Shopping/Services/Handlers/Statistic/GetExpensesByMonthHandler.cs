using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Requests.Statistic;

namespace Shopping.Services.Handlers.Statistic
{
    public sealed class GetExpensesByMonthHandler : IRequestHandler<GetExpensesByMonth, IDictionary<int, decimal>>
    {
        private readonly ShoppingDbContext _context;

        public GetExpensesByMonthHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<IDictionary<int, decimal>> Handle(GetExpensesByMonth request, CancellationToken cancellationToken)
        {
            var firstDayOfYear = request.StartOfYear;
            var lastDayOfYear = request.StartOfYear.AddYears(1).AddSeconds(-1);

            var query = from receiptItem in _context.ReceiptItems
                        join receipt in _context.Receipts on receiptItem.ReceiptId equals receipt.Id
                        where receipt.Date > firstDayOfYear && receipt.Date <= lastDayOfYear
                        select new { Month = receipt.Date.Month, Price = receiptItem.Price, Amount = receiptItem.Amount };

            var data = await query.OrderBy(x => x.Month).ToArrayAsync(cancellationToken).ConfigureAwait(false);
            return data.GroupBy(d => d.Month).ToDictionary(x => x.Key, v => v.Sum(d => (d.Price * d.Amount)));
        }
    }
}