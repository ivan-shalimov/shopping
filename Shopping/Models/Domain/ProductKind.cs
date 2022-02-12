namespace Shopping.Models.Domain
{
    public class ProductKind
    {
        public static readonly Guid DefaultProductKindId = new Guid("272f0bd7-3896-41fe-8c96-b1772519d306");

        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}