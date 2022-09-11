using MediatR;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class DeleteProductKindHandler : IRequestHandler<DeleteProductKind, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public DeleteProductKindHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(DeleteProductKind request, CancellationToken cancellationToken)
        {
            var productKind = await _context.ProductKinds.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (productKind != null)
            {
                _context.ProductKinds.Remove(productKind);
                await _context.SaveChangesAsync();
            }

            return new Success();
        }
    }
}