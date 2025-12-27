using Shopping.Mediator;
using Microsoft.Extensions.DependencyInjection;
using Shopping.DataAccess;
using Shopping.Models.Domain;
using Shopping.Models.Requests;
using Shopping.Services.Interfaces;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class AddReceiptItemHandler : IRequestHandler<AddReceiptItem, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;
        private readonly IBackgroundTaskManager _backgroundTaskManager;

        public AddReceiptItemHandler(ShoppingDbContext context, IBackgroundTaskManager backgroundTaskManager)
        {
            _context = context;
            _backgroundTaskManager = backgroundTaskManager;
        }

        public async Task<Either<Fail, Success>> Handle(AddReceiptItem request, CancellationToken cancellationToken)
        {
            var item = new ReceiptItem
            {
                Id = request.Id,
                ProductId = request.ProductId,
                Amount = request.Amount,
                Price = request.Price,
                ReceiptId = request.ReceiptId,
            };

            await _context.ReceiptItems.AddAsync(item);
            await _context.SaveChangesAsync();

            _backgroundTaskManager.AddTask(async (sp, cnt) =>
            {
                var mediatr = sp.GetService<IMediator>();
                var cmd = new UpdatePriceChangeProjection { ProductId = request.ProductId, ReceiptId = request.ReceiptId };
                await mediatr.Execute(cmd);
            });

            return Success.Instance;
        }
    }
}