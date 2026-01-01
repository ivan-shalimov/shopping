using Shopping.DataAccess;
using Shopping.Mediator;
using Shopping.Models.Requests;
using Shopping.Services.Interfaces;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Handlers.CarCosts
{
    public sealed class UpdateBillItemQuantityHandler : IRequestHandler<UpdateBillItemQuantity, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;
        private readonly IBackgroundMediatorRequestHandler _backgroundRequestHandler;

        public UpdateBillItemQuantityHandler(ShoppingDbContext context, IBackgroundMediatorRequestHandler backgroundRequestHandler)
        {
            _context = context;
            _backgroundRequestHandler = backgroundRequestHandler;
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

            _backgroundRequestHandler.ExecuteInBackground(new UpdateBillTotal { Id = billItem.BillId });

            return Success.Instance;
        }
    }
}