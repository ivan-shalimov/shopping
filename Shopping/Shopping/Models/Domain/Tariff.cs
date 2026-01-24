namespace Shopping.Models.Domain
{
    public class Tariff
    {
        public Guid Id { get; set; }

        public string GroupName { get; set; }

        public string Description { get; set; }

        public RateType RateType { get; set; }

        public bool Quantifiable { get; set; }

        public ICollection<TariffPeriod> TariffPeriods { get; set; }
    }
}