namespace Shopping.Shared.Models.Results
{
    public sealed class ReceiptModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Total { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}