using MediatR;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests
{
    public sealed class UpdateReceiptItem : IRequest<Either<Fail, Success>>
    {
        public Guid Id { get; set; }

        public Guid ReceiptId { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }
    }
}