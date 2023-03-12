using MediatR;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetProductCostChange : IRequest<ProductCostChange[]>
    {
        public int Page { get; set; }

        public int PageSize { get; set; }

        public string OrderBy { get; set; }
    }
}