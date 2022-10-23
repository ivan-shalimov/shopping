namespace Shopping.Shared.Models.Results
{
    public sealed class ProductKindModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool HasProducts { get; set; }
    }
}