using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

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
                        where (product.ProductKindId == request.ProductKindId || !request.ProductKindId.HasValue)
                        && (!product.Hidden || request.ShowHidden)
                        orderby product.Type, product.Name
                        select new ProductModel
                        {
                            Id = product.Id,
                            Name = product.Name,
                            Type = product.Type,
                            ProductKindId = productKind.Id,
                            Hidden = product.Hidden,
                            ProductKindName = productKind.Name
                        };

            var products = await query.ToArrayAsync(cancellationToken).ConfigureAwait(false);

            var hasreceitByKind = (await _context.ReceiptItems
                .Select(p => new { p.Id, p.ProductId })
                .ToArrayAsync(cancellationToken)
                ).GroupBy(p => p.ProductId, p => p.Id)
                .ToDictionary(p => p.Key, p => p.Any());
            foreach (var product in products)
            {
                product.Used = hasreceitByKind.TryGetValue(product.Id, out var used) ? used : false;
            }

            return products;
        }
    }
}