using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class GetReceiptsHandler : IRequestHandler<GetReceipts, Either<Fail, ReceiptModel[]>>
    {
        private readonly ShoppingDbContext _context;

        public GetReceiptsHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, ReceiptModel[]>> Handle(GetReceipts request, CancellationToken cancellationToken)
        {
            var firstDayOfMonth = new DateTime(request.Year, request.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddMinutes(-1);

            var query = from receipt in _context.Receipts
                        orderby receipt.Date descending
                        where receipt.Date > firstDayOfMonth && receipt.Date <= lastDayOfMonth
                        select new ReceiptModel { Id = receipt.Id, Description = receipt.Description, Total = receipt.Total, Date = receipt.Date, };

            var purchases = await query.ToArrayAsync(cancellationToken).ConfigureAwait(false);

            return purchases;
        }
    }
}