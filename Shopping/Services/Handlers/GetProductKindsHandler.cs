using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Results;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class GetProductKindsHandler : IRequestHandler<GetProductKinds, ProductKindModel[]>
    {
        private readonly ShoppingDbContext _context;

        public GetProductKindsHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<ProductKindModel[]> Handle(GetProductKinds request, CancellationToken cancellationToken)
        {
            var purchases = await _context.ProductKinds
                .OrderBy(x => x.Name)
                .Select(p => new ProductKindModel { Id = p.Id, Name = p.Name })
                .ToArrayAsync(cancellationToken).ConfigureAwait(false);

            return purchases;
        }
    }
}