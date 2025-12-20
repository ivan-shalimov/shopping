using Shopping.Mediator;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetExpensesByKind : IRequest<IDictionary<string, decimal>>
    {
        public bool OnlyMain { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}