using MediatR;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests
{
    public sealed class GetPurchaseStatistic : IRequest<PurchaseStatistic>
    {
        public int Month { get; set; }
    }
}