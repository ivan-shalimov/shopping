using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Results;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class GetPurchasesHandler : IRequestHandler<GetPurchases, PurchaseItem[]>
    {
        private readonly ShoppingDbContext _context;

        public GetPurchasesHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseItem[]> Handle(GetPurchases request, CancellationToken cancellationToken)
        {
            var date = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var query = from receiptItem in _context.ReceiptItems
                        join receipt in _context.Receipts on receiptItem.ReceiptId equals receipt.Id
                        join product in _context.Products on receiptItem.ProductId equals product.Id
                        orderby receipt.CreatedOn descending
                        where receipt.CreatedOn > firstDayOfMonth && receipt.CreatedOn <= lastDayOfMonth
                        select new PurchaseItem { Id = receiptItem.Id, Name = product.Name, Price = receiptItem.Price, Amount = receiptItem.Amount };

            var purchases = await query.ToArrayAsync(cancellationToken).ConfigureAwait(false);

            return purchases;
        }
    }
}