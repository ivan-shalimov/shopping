using MediatR;
using Shopping.Models.Results;

namespace Shopping.Requests
{
    public sealed class GetPurchases : IRequest<PurchaseItem[]>
    {
    }
}