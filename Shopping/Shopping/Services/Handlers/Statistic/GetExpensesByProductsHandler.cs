using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests.Statistic;

namespace Shopping.Services.Handlers.Statistic
{
    public sealed class GetExpensesByProductsHandler : IRequestHandler<GetExpensesByProducts, Either<Fail, IDictionary<string, decimal>>>
    {
        private readonly ShoppingDbContext _context;

        public GetExpensesByProductsHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, IDictionary<string, decimal>>> Handle(GetExpensesByProducts request, CancellationToken cancellationToken)
        {
            var query = from receiptItem in _context.ReceiptItems
                        join receipt in _context.Receipts on receiptItem.ReceiptId equals receipt.Id
                        join product in _context.Products on receiptItem.ProductId equals product.Id
                        join productKind in _context.ProductKinds on product.ProductKindId equals productKind.Id
                        where productKind.Name == request.Kind && receipt.Date > request.Start && receipt.Date <= request.End
                        select new { ProductName = product.Name, Price = receiptItem.Price, Amount = receiptItem.Amount };

            var data = await query.OrderBy(x => x.ProductName).ToArrayAsync(cancellationToken).ConfigureAwait(false);
            return data.GroupBy(d => d.ProductName).ToDictionary(x => x.Key, v => v.Sum(d => (d.Price * d.Amount)));
        }
    }
}