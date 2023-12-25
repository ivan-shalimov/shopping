namespace Shopping.Models.Domain
{
    public class CarCost
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }
    }
}