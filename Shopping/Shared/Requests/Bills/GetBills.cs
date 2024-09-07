using MediatR;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests.Bills
{
    public sealed class GetBills : IRequest<Either<Fail, BillModel[]>>
    {
        public int PageSize { get; set; } = 15;

        public int Page { get; set; } = 1;
    }
}