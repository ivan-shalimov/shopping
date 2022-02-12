using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Results;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class GetReceiptsHandler : IRequestHandler<GetReceipts, ReceiptModel[]>
    {
        private readonly ShoppingDbContext _context;

        public GetReceiptsHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<ReceiptModel[]> Handle(GetReceipts request, CancellationToken cancellationToken)
        {
            var date = DateTime.UtcNow;
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var query = from receipt in _context.Receipts
                        orderby receipt.CreatedOn descending
                        where receipt.CreatedOn > firstDayOfMonth && receipt.CreatedOn <= lastDayOfMonth
                        select new ReceiptModel { Id = receipt.Id, Description = receipt.Description, CreatedOn = receipt.CreatedOn, };

            var purchases = await query.ToArrayAsync(cancellationToken).ConfigureAwait(false);

            return purchases;
        }
    }
}