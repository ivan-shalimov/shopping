using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Requests.Statistic;

namespace Shopping.Services.Handlers.Statistic
{
    public sealed class GetExpensesByKindHandler : IRequestHandler<GetExpensesByKind, IDictionary<string, decimal>>
    {
        private readonly ShoppingDbContext _context;

        public GetExpensesByKindHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<IDictionary<string, decimal>> Handle(GetExpensesByKind request, CancellationToken cancellationToken)
        {
            var query = from receiptItem in _context.ReceiptItems
                        join receipt in _context.Receipts on receiptItem.ReceiptId equals receipt.Id
                        join product in _context.Products on receiptItem.ProductId equals product.Id
                        join productKind in _context.ProductKinds on product.ProductKindId equals productKind.Id
                        where (productKind.IsMain || !request.OnlyMain) && receipt.Date > request.Start && receipt.Date <= request.End
                        select new { ProductKindName = productKind.Name, Price = receiptItem.Price, Amount = receiptItem.Amount };

            var data = await query.OrderBy(x=>x.ProductKindName).ToArrayAsync(cancellationToken).ConfigureAwait(false);
            return data.GroupBy(d => d.ProductKindName).ToDictionary(x => x.Key, v => v.Sum(d => (d.Price * d.Amount)));
        }
    }
}