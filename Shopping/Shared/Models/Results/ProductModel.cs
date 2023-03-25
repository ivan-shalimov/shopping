namespace Shopping.Shared.Models.Results
{
    public sealed class ProductModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public Guid ProductKindId { get; set; } // todo check inside = ProductKind.DefaultProductKindId;

        public string ProductKindName { get; set; } = string.Empty;

        public bool Used { get; set; }

        public bool Hidden { get; set; }

        public string Type { get; set; }
    }
}