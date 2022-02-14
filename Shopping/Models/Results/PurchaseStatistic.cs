namespace Shopping.Models.Results
{
    public sealed class PurchaseStatistic
    {
        public Dictionary<string, decimal> Statistics { get; set; } = new Dictionary<string, decimal>();

        public decimal Total => Statistics.Values.Sum();
    }
}