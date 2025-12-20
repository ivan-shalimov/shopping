using Shopping.Mediator;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class AddProductKindHandler : IRequestHandler<AddProductKind, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public AddProductKindHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(AddProductKind request, CancellationToken cancellationToken)
        {
            var item = new ProductKind
            {
                Id = request.Id,
                Name = request.Name,
            };

            await _context.ProductKinds.AddAsync(item);
            await _context.SaveChangesAsync();

            return Success.Instance;
        }
    }
}