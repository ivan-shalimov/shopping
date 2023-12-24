using MediatR;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class AddReceiptItemHandler : IRequestHandler<AddReceiptItem, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public AddReceiptItemHandler(ShoppingDbContext context)
        {
            _context = context;
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

            return Success.Instance;
        }
    }
}