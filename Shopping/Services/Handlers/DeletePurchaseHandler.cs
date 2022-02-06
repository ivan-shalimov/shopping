using MediatR;
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

        public async Task<Unit> Handle(DeletePurchase request, CancellationToken cancellationToken)
        {
            var item = await _context.Purchases.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null)
            {
                _context.Purchases.Remove(item);
                await _context.SaveChangesAsync();
            }

            return default;
        }
    }
}