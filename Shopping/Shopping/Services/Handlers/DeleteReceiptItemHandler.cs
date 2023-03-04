using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class DeleteReceiptItemHandler : IRequestHandler<DeleteReceiptItem>
    {
        private readonly ShoppingDbContext _context;

        public DeleteReceiptItemHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task Handle(DeleteReceiptItem request, CancellationToken cancellationToken)
        {
            var item = await _context.ReceiptItems.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null)
            {
                _context.ReceiptItems.Remove(item);
                await _context.SaveChangesAsync();
            }
        }
    }
}