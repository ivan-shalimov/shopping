using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class UpdateReceiptItemHandler : IRequestHandler<UpdateReceiptItem>
    {
        private readonly ShoppingDbContext _context;

        public UpdateReceiptItemHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateReceiptItem request, CancellationToken cancellationToken)
        {
            var item = await _context.ReceiptItems.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null)
            {
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

                item.Amount = request.Amount;
                item.Price = request.Price;
                await _context.SaveChangesAsync();
            }

            return default;
        }
    }
}