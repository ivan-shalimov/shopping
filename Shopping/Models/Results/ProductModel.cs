using Shopping.Models.Domain;

namespace Shopping.Models.Results
{
    public sealed class ProductModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid ProductKindId { get; set; } = ProductKind.DefaultProductKindId;

        public string ProductKindName { get; set; } = string.Empty;
    }
}