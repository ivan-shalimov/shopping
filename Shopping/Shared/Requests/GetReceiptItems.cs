using Shopping.Mediator;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests
{
    public sealed class GetReceiptItems : IRequest<Either<Fail, ReceiptItemModel[]>>
    {
        public Guid ReceiptId { get; set; }
    }
}