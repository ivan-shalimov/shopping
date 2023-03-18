using MediatR;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetExpensesByProducts : IRequest<IDictionary<string, decimal>>
    {
        public string Kind { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}