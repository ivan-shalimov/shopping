using MediatR;
using Shopping.Models.Results;

namespace Shopping.Requests
{
    public sealed class GetProducts : IRequest<ProductModel[]>
    {
    }
}