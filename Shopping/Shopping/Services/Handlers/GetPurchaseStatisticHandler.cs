using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class GetPurchaseStatisticHandler : IRequestHandler<GetPurchaseStatistic, PurchaseStatistic>
    {
        private readonly ShoppingDbContext _context;

        public GetPurchaseStatisticHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseStatistic> Handle(GetPurchaseStatistic request, CancellationToken cancellationToken)
        {
            var date = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(date.Year, request.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var query = from receiptItem in _context.ReceiptItems
                        join receipt in _context.Receipts on receiptItem.ReceiptId equals receipt.Id
                        join product in _context.Products on receiptItem.ProductId equals product.Id
                        join productKind in _context.ProductKinds on product.ProductKindId equals productKind.Id
                        where receipt.Date > firstDayOfMonth && receipt.Date <= lastDayOfMonth
                        select new { ProductKindName = productKind.Name, Price = receiptItem.Price, Amount = receiptItem.Amount };

            var data = await query.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            return new PurchaseStatistic
            {
                Statistics = data.GroupBy(d => d.ProductKindName).ToDictionary(x => x.Key, v => v.Sum(d => d.Price * d.Amount))
            };
        }
    }
}