using Shopping.Mediator;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Models.Requests;
using Shopping.Shared.Models.Common;

namespace Shopping.Services.Handlers.Prices
{
    public sealed class UpdatePriceChangeProjectionHandler : IRequestHandler<UpdatePriceChangeProjection, Either<Fail, Success>>
    {
        private readonly ShoppingDbContext _context;

        public UpdatePriceChangeProjectionHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<Either<Fail, Success>> Handle(UpdatePriceChangeProjection request, CancellationToken cancellationToken)
        {
            var receipt = await _context.Receipts.FindAsync(new object[] { request.ReceiptId }, cancellationToken);
            if(receipt == null)
            {
                return Success.Instance;
            }

            var lastReceiptItemsQuery = from ri in _context.ReceiptItems
                                        join r in _context.Receipts on ri.ReceiptId equals r.Id
                                        where ri.ProductId == request.ProductId && r.Description == receipt.Description
                                        orderby r.Date descending
                                        select new { r.Description, r.Date, ri.Price };
            var lastReceiptItems = await lastReceiptItemsQuery.Take(2).ToArrayAsync(cancellationToken);

            if (lastReceiptItems.Length == 0)
            {
                return Success.Instance;
            }

            var shop = lastReceiptItems[0].Description;
            var changedDate = lastReceiptItems[0].Date;
            var lastPrice = lastReceiptItems[0].Price;
            var previousPrice = lastReceiptItems.Length == 2 ? lastReceiptItems[1].Price : 0;
            var changePercent = previousPrice == 0 ? 0 : (lastPrice - previousPrice) / previousPrice;

            var product = await _context.Products.FindAsync(new object[] { request.ProductId }, cancellationToken);
            var productKind = await _context.ProductKinds.FindAsync(new object[] { product.ProductKindId }, cancellationToken);

            var priceChange = await _context.PriceChangeProjections
                .Where(pc => pc.ProductId == request.ProductId && pc.Shop == shop)
                .SingleOrDefaultAsync(cancellationToken);
            if (priceChange == null)
            {
                priceChange = new Models.Domain.PriceChangeProjection
                {
                    Id = Guid.NewGuid(),
                    ProductId = request.ProductId,
                    Shop = shop,
                };

                _context.Add(priceChange);
            }

            priceChange.ProductName = product.Name;
            priceChange.ProductKindName = productKind.Name;

            priceChange.PreviousPrice = previousPrice;
            priceChange.ChangedDate = changedDate;
            priceChange.LastPrice = lastPrice;
            priceChange.ChangePercent = changePercent;

            await _context.SaveChangesAsync(cancellationToken);

            return Success.Instance;
        }
    }
}