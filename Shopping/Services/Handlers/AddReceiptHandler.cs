using MediatR;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class AddReceiptHandler : IRequestHandler<AddReceipt>
    {
        private readonly ShoppingDbContext _context;

        public AddReceiptHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(AddReceipt request, CancellationToken cancellationToken)
        {
            var item = new Receipt
            {
                Id = Guid.NewGuid(),
                Description = request.Description,
                CreatedOn = DateTime.UtcNow,
            };

            await _context.Receipts.AddAsync(item);
            await _context.SaveChangesAsync();

            return default;
        }
    }
}