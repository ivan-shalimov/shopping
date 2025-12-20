using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests
{
    public sealed class MergeProductKind : IRequest<Either<Fail, Success>>
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid FirstProductKindId { get; set; }

        public Guid SecondProductKindId { get; set; }

        public string NewProductKindName { get; set; }
    }
}