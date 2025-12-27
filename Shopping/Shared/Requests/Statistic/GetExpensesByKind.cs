using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetExpensesByKind : IRequest<Either<Fail, IDictionary<string, decimal>>>
    {
        public bool OnlyMain { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}