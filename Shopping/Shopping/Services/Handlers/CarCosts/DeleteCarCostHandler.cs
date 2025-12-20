using Shopping.Mediator;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers.CarCosts
{
    public sealed class DeleteCarCostHandler : IRequestHandler<DeleteCarCost, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public DeleteCarCostHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(DeleteCarCost request, CancellationToken cancellationToken)
        {
            var carCost = await _context.CarCosts.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (carCost != null)
            {
                _context.Remove(carCost);
                await _context.SaveChangesAsync();
            }

            return Success.Instance;
        }
    }
}