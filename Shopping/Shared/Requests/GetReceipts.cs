using Shopping.Mediator;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests
{
    public sealed class GetReceipts : IRequest<Either<Fail, ReceiptModel[]>>
    {
        public int Year { get; set; }

        public int Month { get; set; }
    }
}