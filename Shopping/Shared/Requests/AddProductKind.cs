using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests
{
    public sealed class AddProductKind : IRequest<Either<Fail, Success>>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = String.Empty;
    }
}