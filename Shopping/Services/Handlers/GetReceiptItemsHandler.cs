using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Results;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class GetReceiptItemsHandler : IRequestHandler<GetReceiptItems, ReceiptItemModel[]>
    {
        private readonly ShoppingDbContext _context;

        public GetReceiptItemsHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<ReceiptItemModel[]> Handle(GetReceiptItems request, CancellationToken cancellationToken)
        {
            var date = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var query = from receiptItem in _context.ReceiptItems
                        join product in _context.Products on receiptItem.ProductId equals product.Id
                        orderby product.Name
                        where receiptItem.ReceiptId == request.ReceiptId
                        select new ReceiptItemModel { Id = receiptItem.Id, ProductName = product.Name, Price = receiptItem.Price, Amount = receiptItem.Amount };

            var purchases = await query.ToArrayAsync(cancellationToken).ConfigureAwait(false);

            return purchases;
        }
    }
}