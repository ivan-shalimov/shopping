using MediatR;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests
{
    public sealed class GetReceipts : IRequest<ReceiptModel[]>
    {
        public int Month { get; set; }
    }
}