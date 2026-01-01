using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Models.Requests
{
    public sealed class UpdateBillTotal : IRequest<Either<Fail, Success>>
    {
        public Guid Id { get; set; }
    }
}