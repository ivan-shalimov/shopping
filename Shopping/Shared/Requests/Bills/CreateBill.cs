using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests.Bills
{
    public sealed class CreateBill : IRequest<Either<Fail, Success>>
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public required string Description { get; set; }
    }
}