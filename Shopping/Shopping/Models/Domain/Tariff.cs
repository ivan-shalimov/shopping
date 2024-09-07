namespace Shopping.Models.Domain
{
    public class Tariff
    {
        public Guid Id { get; set; }

        public string GroupName { get; set; }

        public string Description { get; set; }

        public decimal Rate  { get; set; }

        public bool Quantifiable { get; set; }

        public DateTime StartOn { get; set; }

        public DateTime? EndOn { get; set; }
}
}