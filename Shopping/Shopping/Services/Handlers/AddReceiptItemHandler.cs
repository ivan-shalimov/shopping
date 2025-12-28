using Shopping.DataAccess;
using Shopping.Mediator;
using Shopping.Models.Domain;
using Shopping.Models.Requests;
using Shopping.Services.Interfaces;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class AddReceiptItemHandler : IRequestHandler<AddReceiptItem, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;
        private readonly IBackgroundRequestHandler _backgroundRequestHandler;

        public AddReceiptItemHandler(ShoppingDbContext context, IBackgroundRequestHandler backgroundRequestHandler)
        {
            _context = context;
            _backgroundRequestHandler = backgroundRequestHandler;
        }

        public async Task<Either<Fail, Success>> Handle(AddReceiptItem request, CancellationToken cancellationToken)
        {
            var item = new ReceiptItem
            {
                Id = request.Id,
                ProductId = request.ProductId,
                Amount = request.Amount,
                Price = request.Price,
                ReceiptId = request.ReceiptId,
            };

            await _context.ReceiptItems.AddAsync(item);
            await _context.SaveChangesAsync();

            _backgroundRequestHandler.ExecuteInBackground(new UpdatePriceChangeProjection { ProductId = request.ProductId, ReceiptId = request.ReceiptId });

            return Success.Instance;
        }
    }
}