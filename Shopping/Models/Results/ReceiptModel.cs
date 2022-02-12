namespace Shopping.Models.Results
{
    public sealed class ReceiptModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
    }
}