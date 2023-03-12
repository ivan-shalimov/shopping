using MediatR;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetExpensesByKind : IRequest<IDictionary<string, double>>
    {
        public DateTime StartOfMonth { get; set; }
    }
}