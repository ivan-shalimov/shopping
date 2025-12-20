using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class MergeProductKindHandler : IRequestHandler<MergeProductKind, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public MergeProductKindHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(MergeProductKind request, CancellationToken cancellationToken)
        {
            var firstProductKind = await _context.ProductKinds.FindAsync(new object[] { request.FirstProductKindId }, cancellationToken);
            var secondProductKind = await _context.ProductKinds.FindAsync(new object[] { request.SecondProductKindId }, cancellationToken);

            var newProductKind = new ProductKind
            {
                Id = request.Id,
                Name = request.NewProductKindName,
            };


            var productsForFirstProductKind = await _context.Products.Where(p => p.ProductKindId == request.FirstProductKindId).ToArrayAsync(cancellationToken);
            foreach (var product in productsForFirstProductKind)
            {
                product.ProductKindId = newProductKind.Id;
            }
            var productsForSecondProductKind = await _context.Products.Where(p => p.ProductKindId == request.SecondProductKindId).ToArrayAsync(cancellationToken);
            foreach (var product in productsForSecondProductKind)
            {
                product.ProductKindId = newProductKind.Id;
            }

            _context.ProductKinds.Remove(firstProductKind);
            _context.ProductKinds.Remove(secondProductKind);
            await _context.ProductKinds.AddAsync(newProductKind);

            await _context.SaveChangesAsync();

            return Success.Instance;
        }
    }
}