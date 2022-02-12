using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class AddPurchaseHandler : IRequestHandler<AddPurchase>
    {
        private readonly ShoppingDbContext _context;

        public AddPurchaseHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddPurchase request, CancellationToken cancellationToken)
        {
            var receipt = new Receipt
            {
                Id = Guid.NewGuid(),
                CreatedOn = DateTime.UtcNow,
            };

            var item = new ReceiptItem
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Price = request.Price,
                ReceiptId = receipt.Id,
            };

            if (request.ProductId.HasValue)
            {
                item.ProductId = request.ProductId.Value;
            }
            else
            {
                var product = request.ProductName?.Length > 0
                    ? await _context.Products.FirstOrDefaultAsync(p => p.Name.ToLower() == request.ProductName.ToLower())
                    : null;
                if (product == null)
                {
                    var entity = await _context.Products.AddAsync(new Product
                    {
                        Id = Guid.NewGuid(),
                        ProductKindId = ProductKind.DefaultProductKindId,
                        Name = request.ProductName ?? Product.UndefinedName
                    });
                    product = entity.Entity;
                }

                item.ProductId = product.Id;
            }

            await _context.Receipts.AddAsync(receipt);
            await _context.ReceiptItems.AddAsync(item);
            await _context.SaveChangesAsync();

            return default;
        }
    }
}