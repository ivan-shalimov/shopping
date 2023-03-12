namespace Shopping.Shared.Models.Results
{
    public class ProductCostChange
    {
        public string Name { get; set; }

        public string Kind { get; set; }

        public string Shop { get; set; }

        public decimal PreviousCost { get; set; }

        public decimal LastCost { get; set; }

        public decimal ChangePercent { get; set; }

        // public double ChangePercent => (LastCost - PreviousCost) / PreviousCost;
    }
}
