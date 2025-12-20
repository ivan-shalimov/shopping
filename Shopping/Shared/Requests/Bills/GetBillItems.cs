using Shopping.Mediator;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests.Bills
{
    public sealed class GetBillItems : IRequest<Either<Fail, BillItemModel[]>>
    {
        public Guid BillId { get; set; }
    }
}