namespace Shopping.Shared.Models.Results
{
    public sealed class BillModel
    {
        public Guid Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public int TotalInt { get; set; }

        public string Total => (TotalInt / 100m).ToString("F2");
    }
}