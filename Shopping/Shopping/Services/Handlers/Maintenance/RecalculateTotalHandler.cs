using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Mediator;
using Shopping.Models.Requests;
using Shopping.Services.Interfaces;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers.Maintenance
{
    public sealed class RecalculateTotalHandler : IRequestHandler<RecalculateTotal, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;
        private readonly IBackgroundMediatorRequestHandler _backgroundRequestHandler;

        public RecalculateTotalHandler(ShoppingDbContext context, IBackgroundMediatorRequestHandler backgroundRequestHandler)
        {
            _context = context;
            _backgroundRequestHandler = backgroundRequestHandler;
        }

        public async Task<Either<Fail, Success>> Handle(RecalculateTotal request, CancellationToken cancellationToken)
        {
            switch (request.Type.ToLowerInvariant())
            {
                case "receipt":
                    await RecalculateReceiptsTotal(request, cancellationToken).ConfigureAwait(false);
                    break;

                case "bill":
                    await RecalculateBillsTotal(request, cancellationToken).ConfigureAwait(false);
                    break;

                default:
                    return new Fail(FailType.NotFound, new[] { "Unknown recalculation type." });
            }

            return Success.Instance;
        }

        private async Task RecalculateBillsTotal(RecalculateTotal request, CancellationToken cancellationToken)
        {
            var firstDayOfYear = new DateTime(request.Year, 1, 1);
            var lastDayOfYear = firstDayOfYear.AddYears(1).AddMinutes(-1);
            var billIds = await _context.Bills
                .Where(b => b.CreatedOn >= firstDayOfYear && b.CreatedOn <= lastDayOfYear)
                .Select(b => b.Id)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);
            foreach (var billId in billIds)
            {
                await _backgroundRequestHandler
                    .ExecuteInBackground(new UpdateBillTotal { Id = billId })
                    .ConfigureAwait(false);
            }
        }

        private async Task RecalculateReceiptsTotal(RecalculateTotal request, CancellationToken cancellationToken)
        {
            var firstDayOfYear = new DateTime(request.Year, 1, 1);
            var lastDayOfYear = firstDayOfYear.AddYears(1).AddMinutes(-1);
            var receiptIds = await _context.Receipts
                .Where(r => r.Date >= firstDayOfYear && r.Date <= lastDayOfYear)
                .Select(r => r.Id)
                .ToArrayAsync(cancellationToken)
                .ConfigureAwait(false);
            foreach (var receiptId in receiptIds)
            {
                await _backgroundRequestHandler
                    .ExecuteInBackground(new UpdateReceiptTotal { Id = receiptId })
                    .ConfigureAwait(false);
            }
        }
    }
}