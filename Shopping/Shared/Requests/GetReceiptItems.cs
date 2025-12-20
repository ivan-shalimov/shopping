using Shopping.Mediator;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests
{
    public sealed class GetReceiptItems : IRequest<ReceiptItemModel[]>
    {
        public Guid ReceiptId { get; set; }
    }
}