using MediatR;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetExpensesByMonth : IRequest<IDictionary<int, double>>
    {
        public DateTime StartOfYear { get; set; }
    }
}