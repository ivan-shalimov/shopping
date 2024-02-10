namespace Shopping.Models.Domain
{
    public class PriceChangeProjection
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public string ProductName { get; set; }

        public string ProductKindName { get; set; }

        public string Shop { get; set; }

        public decimal PreviousPrice { get; set; }

        public decimal LastPrice { get; set; }

        public decimal ChangePercent { get; set; }

        public DateTime ChangedDate { get; set; }

        public byte[] Version { get; set; }
    }
}