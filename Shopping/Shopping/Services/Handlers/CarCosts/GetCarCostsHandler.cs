using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers.CarCosts
{
    public sealed class GetCarCostsHandler : IRequestHandler<GetCarCosts, CarCostModel[]>
    {
        private readonly ShoppingDbContext _context;

        public GetCarCostsHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<CarCostModel[]> Handle(GetCarCosts request, CancellationToken cancellationToken)
        {
            var date = DateTime.UtcNow;
            var startOfMonth = new DateTime(date.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endOfMonth = startOfMonth.AddMonths(1).AddMinutes(-1);

            var carCosts = await _context.CarCosts
                .Where(cc=>cc.Date >= startOfMonth && cc.Date <= endOfMonth )
                .OrderByDescending(x => x.Date)
                .Select(c => new CarCostModel { Id = c.Id, Description = c.Description, Price = c.Price, Amount = c.Amount, Date = c.Date })
                .ToArrayAsync(cancellationToken).ConfigureAwait(false);

            return carCosts;
        }
    }
}