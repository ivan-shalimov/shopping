using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class GetProductKindsHandler : IRequestHandler<GetProductKinds, Either<Fail, ProductKindModel[]>>
    {
        private readonly ShoppingDbContext _context;

        public GetProductKindsHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, ProductKindModel[]>> Handle(GetProductKinds request, CancellationToken cancellationToken)
        {
            var productKinds = await _context.ProductKinds
                .OrderBy(x => x.Name)
                .Select(p => new ProductKindModel { Id = p.Id, Name = p.Name, HasProducts = false })
                .ToArrayAsync(cancellationToken).ConfigureAwait(false);

            var hasProductByKind = (await _context.Products
                .Select(p => new { p.Id, p.ProductKindId })
                .ToArrayAsync(cancellationToken)
                ).GroupBy(p => p.ProductKindId, p => p.Id)
                .ToDictionary(p => p.Key, p => p.Any());
            foreach (var productKind in productKinds)
            {
                productKind.HasProducts = hasProductByKind.TryGetValue(productKind.Id, out var hasProduct) ? hasProduct : false;
            }

            return productKinds;
        }
    }
}