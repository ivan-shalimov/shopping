using MediatR;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class UpdateProductKindHandler : IRequestHandler<UpdateProductKind, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public UpdateProductKindHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(UpdateProductKind request, CancellationToken cancellationToken)
        {
            var item = await _context.ProductKinds.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null)
            {
                item.Name = request.Name;
                await _context.SaveChangesAsync();
            }

            return Success.Instance;
        }
    }
}