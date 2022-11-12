using MediatR;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class UpdateReceiptItemHandler : IRequestHandler<UpdateReceiptItem, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public UpdateReceiptItemHandler(ShoppingDbContext context)
        {
            _context = context;
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

            return new Success();
        }
    }
}