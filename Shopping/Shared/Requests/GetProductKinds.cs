using Shopping.Mediator;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests
{
    public sealed class GetProductKinds : IRequest<Either<Fail, ProductKindModel[]>>
    {
    }
}