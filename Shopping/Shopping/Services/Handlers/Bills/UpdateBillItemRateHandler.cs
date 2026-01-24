using Shopping.DataAccess;
using Shopping.Mediator;
using Shopping.Models.Requests;
using Shopping.Services.Interfaces;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Services.Handlers.Bills
{
    public sealed class UpdateBillItemRateHandler : IRequestHandler<UpdateBillItemRate, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;
        private readonly IBackgroundMediatorRequestHandler _backgroundRequestHandler;

        public UpdateBillItemRateHandler(ShoppingDbContext context, IBackgroundMediatorRequestHandler backgroundRequestHandler)
        {
            _context = context;
            _backgroundRequestHandler = backgroundRequestHandler;
        }

        public async Task<Either<Fail, Success>> Handle(UpdateBillItemRate request, CancellationToken cancellationToken)
        {
            var billItem = await _context.BillItems.FindAsync([request.Id], cancellationToken);

            billItem.Rate = request.Rate;
            await _context.SaveChangesAsync();

            await _backgroundRequestHandler
                .ExecuteInBackground(new UpdateBillTotal { Id = billItem.BillId })
                .ConfigureAwait(false);

            return Success.Instance;
        }
    }
}