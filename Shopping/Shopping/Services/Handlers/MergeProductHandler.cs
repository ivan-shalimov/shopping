using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class MergeProductHandler : IRequestHandler<MergeProduct, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public MergeProductHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(MergeProduct request, CancellationToken cancellationToken)
        {
            var savedProduct = await _context.Products.FindAsync(new object[] { request.SavedProductId }, cancellationToken);
            var removedProductId = await _context.Products.FindAsync(new object[] { request.RemovedProductId }, cancellationToken);

            var receiptItemsForRemovedProduct = await _context.ReceiptItems.Where(p => p.ProductId == removedProductId.Id).ToArrayAsync(cancellationToken);
            foreach (var receiptItem in receiptItemsForRemovedProduct)
            {
                receiptItem.ProductId = savedProduct.Id;
            }

            _context.Remove(removedProductId);

            await _context.SaveChangesAsync();

            return Success.Instance;
        }
    }
}