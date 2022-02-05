namespace Shopping.Models.Domain
{
    public class Purchase
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Cost { get; set; }

        public DateTime Created { get; set; }
    }
}