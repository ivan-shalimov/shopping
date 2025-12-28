using Shopping.DataAccess;
using Shopping.Mediator;
using Shopping.Models.Requests;
using Shopping.Services.Interfaces;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class UpdateReceiptItemHandler : IRequestHandler<UpdateReceiptItem, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;
        private readonly IBackgroundRequestHandler _backgroundRequestHandler;

        public UpdateReceiptItemHandler(ShoppingDbContext context, IBackgroundRequestHandler backgroundRequestHandler)
        {
            _context = context;
            _backgroundRequestHandler = backgroundRequestHandler;
        }

        public async Task<Either<Fail, Success>> Handle(UpdateReceiptItem request, CancellationToken cancellationToken)
        {
            var item = await _context.ReceiptItems.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null) // todo add validation for existing
            {
                item.Amount = request.Amount;
                item.Price = request.Price;
                await _context.SaveChangesAsync();
            }

            _backgroundRequestHandler.ExecuteInBackground(new UpdatePriceChangeProjection { ProductId = item.ProductId, ReceiptId = item.ReceiptId });

            return Success.Instance;
        }
    }
}