using MediatR;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class DeleteProductHandler : IRequestHandler<DeleteProduct, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public DeleteProductHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(DeleteProduct request, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return Success.Instance;
        }
    }
}