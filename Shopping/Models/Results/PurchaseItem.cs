namespace Shopping.Models.Results
{
    public class PurchaseItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }

        public decimal Cost => Price * Amount;
    }
}