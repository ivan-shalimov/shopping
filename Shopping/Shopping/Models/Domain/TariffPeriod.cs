namespace Shopping.Models.Domain
{
    public class TariffPeriod
    {
        public Guid Id { get; set; }

        public Guid TariffId { get; set; }

        public Tariff Tariff { get; set; }

        public decimal Rate  { get; set; }

        public DateTime StartOn { get; set; }

        public DateTime? EndOn { get; set; }
}
}