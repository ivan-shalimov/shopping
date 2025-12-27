using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetExpensesByMonth : IRequest<Either<Fail, IDictionary<int, decimal>>>
    {
        public DateTime StartOfYear { get; set; }
    }
}