namespace Shopping.Shared.Models.Results
{
    public sealed class ReceiptItemModel
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string ProductType { get; set; }

        public string ProductName { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }

        public decimal Cost => Price * Amount;
    }
}