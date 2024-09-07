namespace Shopping.Models.Domain
{
    public class Bill
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}