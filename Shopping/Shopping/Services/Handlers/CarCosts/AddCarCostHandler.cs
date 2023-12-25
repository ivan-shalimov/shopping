using MediatR;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers.CarCosts
{
    public sealed class AddCarCostHandler : IRequestHandler<AddCarCost, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public AddCarCostHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(AddCarCost request, CancellationToken cancellationToken)
        {
            var item = new CarCost
            {
                Id = request.Id,
                Description = request.Description,
                Amount = request.Amount,
                Price = request.Price,
                Date = request.Date,
            };

            await _context.AddAsync(item);
            await _context.SaveChangesAsync();

            return Success.Instance;
        }
    }
}