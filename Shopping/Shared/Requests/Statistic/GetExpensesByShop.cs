using Shopping.Mediator;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetExpensesByShop : IRequest<IDictionary<string, decimal>>
    {
        public DateTime StartOfMonth { get; set; }
    }
}