using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests.Prices
{
    public sealed class GetLastProductsPrices : IRequest<Either<Fail, IDictionary<Guid, decimal>>>
    {
        public Guid ReceiptId { get; set; }

        public Guid[] ProductIds { get; set; }
    }
}