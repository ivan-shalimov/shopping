using MediatR;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class AddProductKindHandler : IRequestHandler<AddProductKind>
    {
        private readonly ShoppingDbContext _context;

        public AddProductKindHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddProductKind request, CancellationToken cancellationToken)
        {
            var item = new ProductKind
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
            };

            await _context.ProductKinds.AddAsync(item);
            await _context.SaveChangesAsync();

            return default;
        }
    }
}