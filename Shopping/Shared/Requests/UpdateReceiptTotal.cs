using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests
{
    public sealed class UpdateReceiptTotal : IRequest<Either<Fail, Success>>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}