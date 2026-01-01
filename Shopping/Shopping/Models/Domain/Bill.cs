namespace Shopping.Models.Domain
{
    public class Bill
    {
        public Guid Id { get; set; }

        public string Description { get; set; }

        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Stores the total amount of the bill bases on minor currency unit.
        /// </summary>
        public int Total { get; set; }
    }
}