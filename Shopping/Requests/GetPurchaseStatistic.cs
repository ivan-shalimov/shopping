using MediatR;
using Shopping.Models;

namespace Shopping.Requests
{
    public sealed class GetPurchaseStatistic : IRequest<PurchaseStatistic>
    {
    }
}