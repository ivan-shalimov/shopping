using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetExpensesByShop : IRequest<Either<Fail, IDictionary<string, decimal>>>
    {
        public DateTime StartOfMonth { get; set; }
    }
}