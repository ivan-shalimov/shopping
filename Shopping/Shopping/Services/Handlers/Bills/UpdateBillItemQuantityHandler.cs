using Shopping.Mediator;
using Shopping.DataAccess;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Handlers.CarCosts
{
    public sealed class UpdateBillItemQuantityHandler : IRequestHandler<UpdateBillItemQuantity, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public UpdateBillItemQuantityHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(UpdateBillItemQuantity request, CancellationToken cancellationToken)
        {
            var billItem = await _context.BillItems.FindAsync([request.Id], cancellationToken);
            if (billItem == null)
            {
                return new Fail(FailType.Validation, ["Bill item is not found"]);
            }

            billItem.Quantity = request.Quantity;
            await _context.SaveChangesAsync();

            return Success.Instance;
        }
    }
}