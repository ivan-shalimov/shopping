using MediatR;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class AddProductHandler : IRequestHandler<AddProduct>
    {
        private readonly ShoppingDbContext _context;

        public AddProductHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddProduct request, CancellationToken cancellationToken)
        {
            var item = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                ProductKindId = request.ProductKindId,
            };

            await _context.Products.AddAsync(item);
            await _context.SaveChangesAsync();

            return default;
        }
    }
}