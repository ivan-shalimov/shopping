using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Handlers.CarCosts
{
    public sealed class GetBillsHandler : IRequestHandler<GetBills, Either<Fail, BillModel[]>>
    {
        private readonly ShoppingDbContext _context;

        public GetBillsHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, BillModel[]>> Handle(GetBills request, CancellationToken cancellationToken)
        {
            var result = await _context.Bills
                .OrderByDescending(e => e.CreatedOn)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(e => new BillModel
                {
                    Id = e.Id,
                    Description = e.Description,
                })
                .ToArrayAsync(cancellationToken);

            return result;
        }
    }
}