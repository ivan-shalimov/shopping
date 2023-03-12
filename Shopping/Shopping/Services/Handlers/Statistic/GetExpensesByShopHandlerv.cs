using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Requests.Statistic;

namespace Shopping.Services.Handlers.Statistic
{
    public sealed class GetExpensesByShopHandler : IRequestHandler<GetExpensesByShop, IDictionary<string, double>>
    {
        private readonly ShoppingDbContext _context;

        public GetExpensesByShopHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<IDictionary<string, double>> Handle(GetExpensesByShop request, CancellationToken cancellationToken)
        {
            var firstDayOfMonth = request.StartOfMonth;
            var lastDayOfMonth = request.StartOfMonth.AddMonths(1).AddSeconds(-1);

            var query = from receiptItem in _context.ReceiptItems
                        join receipt in _context.Receipts on receiptItem.ReceiptId equals receipt.Id
                        where receipt.Date > firstDayOfMonth && receipt.Date <= lastDayOfMonth
                        select new { Shop = receipt.Description, Price = receiptItem.Price, Amount = receiptItem.Amount };

            var data = await query.OrderBy(x => x.Shop).ToArrayAsync(cancellationToken).ConfigureAwait(false);
            return data.GroupBy(d => d.Shop).ToDictionary(x => x.Key, v => v.Sum(d => (double)(d.Price * d.Amount)));
        }
    }
}