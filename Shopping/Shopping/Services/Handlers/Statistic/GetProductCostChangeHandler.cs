using MediatR;
using Microsoft.EntityFrameworkCore;
using Shopping.DataAccess;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests.Statistic;

namespace Shopping.Services.Handlers.Statistic
{
    public sealed class GetProductCostChangeHandler : IRequestHandler<GetProductCostChange, ProductCostChange[]>
    {
        private readonly ShoppingDbContext _context;

        public GetProductCostChangeHandler(ShoppingDbContext context)
        {
            _context = context;
        }

        public async Task<ProductCostChange[]> Handle(GetProductCostChange request, CancellationToken cancellationToken)
        {
            var now = DateTime.UtcNow;
            var twoMonthAgo = now.AddMonths(-2);
            var priceChangeQuery = from receiptItem in _context.ReceiptItems
                                   join receipt in _context.Receipts on receiptItem.ReceiptId equals receipt.Id
                                   join secondReceiptItem in _context.ReceiptItems on receiptItem.ProductId equals secondReceiptItem.ProductId
                                   join secondReceipt in _context.Receipts on secondReceiptItem.ReceiptId equals secondReceipt.Id
                                   where twoMonthAgo < receipt.Date && receipt.Date < now
                                   && receiptItem.Price != secondReceiptItem.Price
                                   && secondReceipt.Date < receipt.Date && secondReceipt.Description == receipt.Description
                                   orderby receipt.Date
                                   select new
                                   {
                                       ProductId = receiptItem.ProductId,
                                       LastDate = receipt.Date,
                                       LastPrice = receiptItem.Price,
                                       PreviousPrice = secondReceiptItem.Price,
                                       Shop = receipt.Description,
                                   };
            var allPriceChange = await priceChangeQuery.ToArrayAsync();

            var temp = allPriceChange.GroupBy(x => $"{x.Shop}_{x.ProductId}")
                .Select(x => x.OrderBy(x => x.LastDate).Last())
                .Select(x => new { x.ProductId, x.PreviousPrice, x.LastPrice, x.Shop, ChangePercent = (x.LastPrice - x.PreviousPrice) / x.PreviousPrice });

            var priceChange = allPriceChange.GroupBy(x => $"{x.Shop}_{x.ProductId}")
                .Select(x => x.OrderBy(x => x.LastDate).Last())
                .Select(x => new { x.ProductId, x.PreviousPrice, x.LastPrice, x.Shop, ChangePercent = (x.LastPrice - x.PreviousPrice) / x.PreviousPrice })
                .OrderByDescending(x => x.ChangePercent)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToArray();

            var productIds = priceChange.Select(x => x.ProductId).ToArray();
            var productDictionary = await (from product in _context.Products
                                           join productKind in _context.ProductKinds on product.ProductKindId equals productKind.Id
                                           where productIds.Contains(product.Id)
                                           select new { product.Id, product.Name, Kind = productKind.Name })
                                     .ToDictionaryAsync(x => x.Id);

            var result = priceChange.Select(x => new ProductCostChange
            {
                Name = productDictionary[x.ProductId].Name,
                Kind = productDictionary[x.ProductId].Kind,
                Shop = x.Shop,
                LastCost = x.LastPrice,
                PreviousCost = x.PreviousPrice,
                ChangePercent = x.ChangePercent
            }).ToArray();

            return result;
        }
    }
}