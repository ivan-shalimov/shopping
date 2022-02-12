namespace Shopping.Models.Domain
{
    public class Product
    {
        public static string UndefinedName = "Undefined";

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid ProductKindId { get; set; }
    }
}