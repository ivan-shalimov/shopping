using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Requests.Prices;

namespace Shopping.Services.Handlers.Prices
{
    public sealed class GetLastProductsPricesHandler : IRequestHandler<GetLastProductsPrices, IDictionary<Guid, decimal>>
    {
        private readonly ShoppingDbContext _context;

        public GetLastProductsPricesHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<IDictionary<Guid, decimal>> Handle(GetLastProductsPrices request, CancellationToken cancellationToken)
        {
            var shopName = (await _context.Receipts.FindAsync(new object[] { request.ReceiptId }, cancellationToken))
                .Description.Trim();

            var query = from receiptItem in _context.ReceiptItems
                        join receipt in _context.Receipts on receiptItem.ReceiptId equals receipt.Id
                        where receipt.Description.Trim() == shopName && request.ProductIds.Contains(receiptItem.ProductId)
                        select new { receipt.Date, receiptItem.ProductId, receiptItem.Price };

            var data = await query.ToArrayAsync(cancellationToken).ConfigureAwait(false);

            return data.GroupBy(d => d.ProductId).ToDictionary(x => x.Key, v => v.OrderBy(x => x.Date).Last().Price);
        }
    }
}