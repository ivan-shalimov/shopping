using MediatR;
using Shopping.Models.Results;

namespace Shopping.Requests
{
    public sealed class AddPurchase : IRequest
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }
    }
}