using MediatR;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetExpensesByShop : IRequest<IDictionary<string, double>>
    {
        public DateTime StartOfMonth { get; set; }
    }
}