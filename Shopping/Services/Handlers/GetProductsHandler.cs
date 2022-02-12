using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Results;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class GetProductsHandler : IRequestHandler<GetProducts, ProductModel[]>
    {
        private readonly ShoppingDbContext _context;

        public GetProductsHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<ProductModel[]> Handle(GetProducts request, CancellationToken cancellationToken)
        {
            var query = from product in _context.Products
                        join productKind in _context.ProductKinds on product.ProductKindId equals productKind.Id
                        orderby product.Name 
                        select new ProductModel { Id = product.Id, Name = product.Name, ProductKindId = productKind.Id, ProductKindName = productKind.Name };

            var purchases = await query.ToArrayAsync(cancellationToken).ConfigureAwait(false);

            return purchases;
        }
    }
}