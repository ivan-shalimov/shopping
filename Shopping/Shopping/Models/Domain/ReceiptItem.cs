namespace Shopping.Models.Domain
{
    public class ReceiptItem
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Guid ReceiptId { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }
    }
}