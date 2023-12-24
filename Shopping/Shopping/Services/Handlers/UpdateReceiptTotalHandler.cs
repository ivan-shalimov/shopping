using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class UpdateReceiptTotalHandler : IRequestHandler<UpdateReceiptTotal,Success>
    {
        private readonly ShoppingDbContext _context;

        public UpdateReceiptTotalHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Success> Handle(UpdateReceiptTotal request, CancellationToken cancellationToken)
        {
            var item = await _context.Receipts.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            var total = await _context.ReceiptItems
                .Where(ri => ri.ReceiptId == request.Id)
                .SumAsync(ri => ri.Price * ri.Amount).ConfigureAwait(false);
            if (item != null)
            {
                item.Total = total;
                await _context.SaveChangesAsync();
            }

            return Success.Instance;
        }
    }
}