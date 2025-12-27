using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests
{
    public sealed class AddProduct : IRequest<Either<Fail, Success>>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProductKindId { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string? Type { get; set; }
    }
}