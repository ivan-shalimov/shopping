using Shopping.Mediator;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests
{
    public sealed class GetCarCosts : IRequest<Either<Fail, CarCostModel[]>>
    {
        public int Month { get; set; }
    }
}