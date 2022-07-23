using MediatR;
using Shopping.DataAccess;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class UpdateReceiptHandler : IRequestHandler<UpdateReceipt>
    {
        private readonly ShoppingDbContext _context;

        public UpdateReceiptHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateReceipt request, CancellationToken cancellationToken)
        {
            var item = await _context.Receipts.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null)
            {
                item.Description = request.Description;
                item.Date = request.Date;
                await _context.SaveChangesAsync();
            }

            return default;
        }
    }
}