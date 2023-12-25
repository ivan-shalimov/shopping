using MediatR;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers.CarCosts
{
    public sealed class UpdateCarCostHandler : IRequestHandler<UpdateCarCost, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public UpdateCarCostHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(UpdateCarCost request, CancellationToken cancellationToken)
        {
            var item = await _context.CarCosts.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null)
            {
                item.Description = request.Description;
                item.Price = request.Price;
                item.Amount = request.Amount;
                item.Date = request.Date;
                await _context.SaveChangesAsync();
            }

            return Success.Instance;
        }
    }
}