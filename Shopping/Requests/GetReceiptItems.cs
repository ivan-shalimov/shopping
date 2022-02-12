using MediatR;
using Shopping.Models.Results;

namespace Shopping.Requests
{
    public sealed class GetReceiptItems : IRequest<ReceiptItemModel[]>
    {
        public Guid ReceiptId { get; set; }
    }
}