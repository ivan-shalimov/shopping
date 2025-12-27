using Shopping.Mediator;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class AddProductHandler : IRequestHandler<AddProduct, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public AddProductHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(AddProduct request, CancellationToken cancellationToken)
        {
            var item = new Product
            {
                Id = request.Id,
                Type = request.Type,
                Name = request.Name,
                ProductKindId = request.ProductKindId,
            };

            await _context.Products.AddAsync(item);
            await _context.SaveChangesAsync();

            return Success.Instance;
        }
    }
}