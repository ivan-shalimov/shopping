using Shopping.Mediator;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests
{
    public sealed class GetPurchaseStatistic : IRequest<Either<Fail, PurchaseStatistic>>
    {
        public int Month { get; set; }
    }
}