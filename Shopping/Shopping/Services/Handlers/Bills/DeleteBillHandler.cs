using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Handlers.Bills
{
    public sealed class DeleteBillHandler : IRequestHandler<DeleteBill, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public DeleteBillHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(DeleteBill request, CancellationToken cancellationToken)
        {
            var bill = await _context.Bills.FindAsync([request.Id], cancellationToken);
            if (bill == null)
            {
                return new Fail(FailType.Validation, ["Bill is not found."]);
            }

            var items = await _context.BillItems.Where(e => e.BillId == request.Id).ToArrayAsync(cancellationToken);

            _context.Remove(bill);
            _context.RemoveRange(items);
            await _context.SaveChangesAsync();

            return Success.Instance;
        }
    }
}