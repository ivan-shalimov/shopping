using MediatR;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class ChangeProductVisibilityHandler : IRequestHandler<ChangeProductVisibility, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public ChangeProductVisibilityHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(ChangeProductVisibility request, CancellationToken cancellationToken)
        {
            var item = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null)
            {
                item.Hidden = request.Hidden;
                await _context.SaveChangesAsync();
            }

            return new Success();
        }
    }
}