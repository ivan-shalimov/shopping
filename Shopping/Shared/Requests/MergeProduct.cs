using Shopping.Mediator;
using Shopping.Shared.Models.Common;

namespace Shopping.Shared.Requests
{
    public sealed class MergeProduct : IRequest<Either<Fail, Success>>
    {
        public Guid SavedProductId { get; set; }

        public Guid RemovedProductId { get; set; }
    }
}