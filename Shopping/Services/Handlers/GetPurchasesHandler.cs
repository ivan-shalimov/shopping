using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Results;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class GetPurchasesHandler : IRequestHandler<GetPurchases, PurchaseItem[]>
    {
        private readonly ShoppingDbContext _context;

        public GetPurchasesHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseItem[]> Handle(GetPurchases request, CancellationToken cancellationToken)
        {
            var purchases = await _context.Purchases
                .OrderByDescending(d => d.Created)
                .Select(p => new PurchaseItem { Id = p.Id, Name = p.Name, Price = p.Price, Amount = p.Amount })
                .ToArrayAsync(cancellationToken).ConfigureAwait(false);

            return purchases;
        }
    }
}