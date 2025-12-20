using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests.Statistic;

namespace Shopping.Services.Handlers.Statistic
{
    public sealed class GetProductCostChangeHandler : IRequestHandler<GetProductCostChange, ProductCostChange[]>
    {
        private readonly ShoppingDbContext _context;

        public GetProductCostChangeHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<ProductCostChange[]> Handle(GetProductCostChange request, CancellationToken cancellationToken)
        {
            var result = await _context.PriceChangeProjections
                .OrderByDescending(pc => pc.ChangedDate)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(pc => new ProductCostChange
                {
                    Name = pc.ProductName,
                    Kind = pc.ProductKindName,
                    Shop = pc.Shop,
                    LastCost = pc.LastPrice,
                    PreviousCost = pc.PreviousPrice,
                    ChangePercent = pc.ChangePercent
                }).ToArrayAsync(cancellationToken);

            return result;
        }
    }
}