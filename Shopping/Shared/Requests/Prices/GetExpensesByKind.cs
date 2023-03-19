using MediatR;

namespace Shopping.Shared.Requests.Prices
{
    public sealed class GetLastProductsPrices : IRequest<IDictionary<Guid, decimal>>
    {
        public Guid ReceiptId { get; set; }

        public Guid[] ProductIds { get; set; }
    }
}