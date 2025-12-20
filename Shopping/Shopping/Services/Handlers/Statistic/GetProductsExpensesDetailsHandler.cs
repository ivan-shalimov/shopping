using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests.Statistic;

namespace Shopping.Services.Handlers.Statistic
{
    public sealed class GetProductsExpensesDetailsHandler : IRequestHandler<GetProductsExpensesDetails, ProductExpensesDetail[]>
    {
        private readonly ShoppingDbContext _context;

        public GetProductsExpensesDetailsHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<ProductExpensesDetail[]> Handle(GetProductsExpensesDetails request, CancellationToken cancellationToken)
        {
            var query = from receiptItem in _context.ReceiptItems
                        join receipt in _context.Receipts on receiptItem.ReceiptId equals receipt.Id
                        join product in _context.Products on receiptItem.ProductId equals product.Id
                        join productKind in _context.ProductKinds on product.ProductKindId equals productKind.Id
                        where product.Name == request.ProductName && receipt.Date > request.Start && receipt.Date <= request.End
                        select new ProductExpensesDetail
                        {
                            ShopName = receipt.Description,
                            SpentOn = receipt.Date,
                            Price = receiptItem.Price,
                            Amount = receiptItem.Amount
                        };

            var data = await query.OrderBy(x => x.SpentOn).ToArrayAsync(cancellationToken).ConfigureAwait(false);
            return data;
        }
    }
}