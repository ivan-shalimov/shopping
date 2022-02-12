namespace Shopping.Models.Domain
{
    public class Receipt
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = " - ";

        public decimal Total { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}