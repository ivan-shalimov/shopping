using MediatR;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests
{
    public sealed class GetProductKinds : IRequest<ProductKindModel[]>
    {
    }
}