using MediatR;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetExpensesByKind : IRequest<IDictionary<string, decimal>>
    {
        public DateTime StartOfMonth { get; set; }
    }
}