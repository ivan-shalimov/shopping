using MediatR;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests.Bills
{
    public sealed class UpdateBillItemQuantity : IRequest<Either<Fail, Success>>
    {
        public Guid Id { get; set; }

        public Guid BillId { get; set; }

        public int Quantity { get; set; }
    }
}