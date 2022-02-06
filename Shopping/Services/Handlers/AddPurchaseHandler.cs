using MediatR;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class AddPurchaseHandler : IRequestHandler<AddPurchase>
    {
        private readonly ShoppingDbContext _context;

        public AddPurchaseHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddPurchase request, CancellationToken cancellationToken)
        {
            var item = new Purchase
            {
                Id = Guid.NewGuid(),
                Amount = request.Amount,
                Name = request.Name,
                Price = request.Price,
                Created = DateTime.UtcNow,
            };

            await _context.Purchases.AddAsync(item);
            await _context.SaveChangesAsync();

            return default;
        }
    }
}