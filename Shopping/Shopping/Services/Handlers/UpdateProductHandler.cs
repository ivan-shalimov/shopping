using Shopping.Mediator;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class UpdateProductHandler : IRequestHandler<UpdateProduct,Success>
    {
        private readonly ShoppingDbContext _context;

        public UpdateProductHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Success> Handle(UpdateProduct request, CancellationToken cancellationToken)
        {
            var item = await _context.Products.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null)
            {
                item.Type = request.Type;
                item.Name = request.Name;
                item.ProductKindId = request.ProductKindId;
                await _context.SaveChangesAsync();
            }

            return Success.Instance;
        }
    }
}