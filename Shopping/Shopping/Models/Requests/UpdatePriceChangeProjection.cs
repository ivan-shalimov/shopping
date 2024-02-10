using MediatR;
using Shopping.Shared.Models.Common;

namespace Shopping.Models.Requests
{
    public sealed class UpdatePriceChangeProjection : IRequest<Either<Fail, Success>>
    {
        public Guid ProductId { get; set; }

        public Guid ReceiptId { get; set; }
    }
}