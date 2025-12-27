using Shopping.Mediator;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetProductCostChange : IRequest<Either<Fail, ProductCostChange[]>>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}