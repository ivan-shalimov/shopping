using MediatR;
using Shopping.DataAccess;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class UpdateProductKindHandler : IRequestHandler<UpdateProductKind>
    {
        private readonly ShoppingDbContext _context;

        public UpdateProductKindHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProductKind request, CancellationToken cancellationToken)
        {
            var item = await _context.ProductKinds.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null)
            {
                item.Name = request.Name;
                await _context.SaveChangesAsync();
            }

            return default;
        }
    }
}