using MediatR;
using Shopping.Models;
using Shopping.Requests;

namespace Shopping.Services.Handlers
{
    public sealed class GetPurchaseStatisticHandler : IRequestHandler<GetPurchaseStatistic, PurchaseStatistic>
    {
        public Task<PurchaseStatistic> Handle(GetPurchaseStatistic request, CancellationToken cancellationToken)
        {
            return Task.FromResult(new PurchaseStatistic
            {
                Statistics = new Dictionary<string, decimal>
                {
                    { "Fruit", 423.20m },
                    { "Vegetables", 223.20m },
                    { "Sweet-stuff", 823.20m },
                    { "Fish", 123.20m },
                    { "Meet", 923.20m },
                    { "Milks", 483.20m },
                }
            });
        }
    }
}