using MediatR;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests.Bills
{
    public sealed class DeleteBill : IRequest<Either<Fail, Success>>
    {
        public Guid Id { get; set; }
    }
}