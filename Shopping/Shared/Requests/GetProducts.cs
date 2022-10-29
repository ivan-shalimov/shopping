using MediatR;
using Shopping.Shared.Models.Results;

namespace Shopping.Shared.Requests
{
    public sealed class GetProducts : IRequest<ProductModel[]>
    {
        public Guid? ProductKindId { get; set; }
    }
}