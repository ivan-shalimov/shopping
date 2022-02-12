using MediatR;

namespace Shopping.Requests
{
    public sealed class AddReceiptItem : IRequest
    {
        public Guid Id { get; set; }

        public Guid ReceiptId { get; set; }

        public Guid? ProductId { get; set; }

        public string? ProductName { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }
    }
}