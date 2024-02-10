using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Shopping.DataAccess;
using Shopping.Models.Requests;
using Shopping.Services.Common;
using Shopping.Services.Interfaces;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class UpdateReceiptItemHandler : IRequestHandler<UpdateReceiptItem, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;
        private readonly IBackgroundTaskManager _backgroundTaskManager;

        public UpdateReceiptItemHandler(ShoppingDbContext context, IBackgroundTaskManager backgroundTaskManager)
        {
            _context = context;
            _backgroundTaskManager = backgroundTaskManager;
        }

        public async Task<Either<Fail, Success>> Handle(UpdateReceiptItem request, CancellationToken cancellationToken)
        {
            var item = await _context.ReceiptItems.FindAsync(new object[] { request.Id }, cancellationToken).ConfigureAwait(false);
            if (item != null) // todo add validation for existing
            {
                item.Amount = request.Amount;
                item.Price = request.Price;
                await _context.SaveChangesAsync();
            }

            _backgroundTaskManager.AddTask(async (sp, cnt) =>
            {
                var mediatr = sp.GetService<IMediator>();
                await mediatr.Send(new UpdatePriceChangeProjection { ProductId = item.ProductId, ReceiptId = item.ReceiptId }, cnt);
            });

            return Success.Instance;
        }
    }
}