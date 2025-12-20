using Shopping.Mediator;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests.Statistic
{
    public sealed class GetProductsExpensesDetails : IRequest<ProductExpensesDetail[]>
    {
        public string ProductName { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }
    }
}