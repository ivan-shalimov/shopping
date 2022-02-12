using MediatR;
using Shopping.Models.Results;

namespace Shopping.Requests
{
    public sealed class AddPurchase : IRequest
    {
        public Guid? ProductId { get; set; }

        public string? ProductName { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }
    }
}