using Shopping.Mediator;
using Shopping.Shared.Models.Common;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests
{
    public sealed class GetProducts : IRequest<Either<Fail, ProductModel[]>>
    {
        public Guid? ProductKindId { get; set; }

        public bool ShowHidden { get; set; }
    }
}