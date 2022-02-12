using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class DeletePurchaseHandler : IRequestHandler<DeletePurchase>
    {
        private readonly ShoppingDbContext _context;

        public DeletePurchaseHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        // todo shhould be removed
        public async Task<Unit> Handle(DeletePurchase request, CancellationToken cancellationToken)
        {
            var item = await _context.ReceiptItems.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null)
            {
                _context.ReceiptItems.Remove(item);

                if (!await _context.ReceiptItems.AnyAsync(ri => ri.ReceiptId == item.ReceiptId && item.Id != item.Id))
                {
                    var receipt = await _context.Receipts.FindAsync(new object[] { item.ReceiptId }, cancellationToken).ConfigureAwait(false);
                    if (receipt != null)
                    {
                        _context.Receipts.Remove(receipt);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return default;
        }
    }
}