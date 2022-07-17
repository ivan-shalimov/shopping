using MediatR;
using Shopping.Models.Results;

namespace Shopping.Requests
{
    public sealed class GetPurchaseStatistic : IRequest<PurchaseStatistic>
    {
        public int Month { get; set; }
    }
}