namespace Shopping.Models.Domain
{
    public class BillItem
    {
        public Guid Id { get; set; }

        public Guid BillId { get; set; }

        public Guid TariffId { get; set; }

        public decimal Rate { get; set; }

        public int PreviousValue { get; set; }

        public int Quantity { get; set; }
    }
}