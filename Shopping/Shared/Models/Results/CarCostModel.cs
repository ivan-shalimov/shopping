namespace Shopping.Shared.Models.Results
{
    public sealed class CarCostModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public decimal Amount { get; set; }

        public decimal Cost => Price * Amount;

        public DateTime Date { get; set; }
    }
}