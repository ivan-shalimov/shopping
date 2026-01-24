using System.Text.Json.Serialization;

namespace Shopping.Shared.Models.Results
{
    public sealed class BillItemModel
    {
        public Guid Id { get; set; }

        public Guid BillId { get; set; }

        public string Group { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int PreviousValue { get; set; }

        [JsonIgnore]
        public int CurrentValue
        {
            get => PreviousValue + Quantity;
            set => Quantity = value - PreviousValue;
        }

        public decimal Rate { get; set; }

        public bool Quantifiable { get; set; }

        public int Quantity { get; set; }

        [JsonIgnore]
        public decimal Charge => Quantifiable ? Rate * Quantity : Rate;

        public bool IsReadonlyRate { get; set; }
    }
}