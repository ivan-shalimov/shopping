using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class GetPurchaseStatisticHandler : IRequestHandler<GetPurchaseStatistic, PurchaseStatistic>
    {
        private readonly ShoppingDbContext _context;

        public GetPurchaseStatisticHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<PurchaseStatistic> Handle(GetPurchaseStatistic request, CancellationToken cancellationToken)
        {
            var data = await _context.Purchases.ToArrayAsync(cancellationToken).ConfigureAwait(false);
            return new PurchaseStatistic
            {
                Statistics = data.ToDictionary(x => x.Name, v => v.Cost)
            };
        }
    }
}