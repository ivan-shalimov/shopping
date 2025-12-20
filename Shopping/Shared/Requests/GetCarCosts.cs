using Shopping.Mediator;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests
{
    public sealed class GetCarCosts : IRequest<CarCostModel[]>
    {
        public int Month { get; set; }
    }
}