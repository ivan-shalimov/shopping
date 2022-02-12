using MediatR;
using Shopping.DataAccess;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class UpdateProductHandler : IRequestHandler<UpdateProduct>
    {
        private readonly ShoppingDbContext _context;

        public UpdateProductHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            var item = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null)
            {
                item.Name = request.Name;
                item.ProductKindId = request.ProductKindId;
                await _context.SaveChangesAsync();
            }

            return default;
        }
    }
}