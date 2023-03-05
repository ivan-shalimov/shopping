using Shopping.Shared.Models.Results;

namespace Shopping.Client.Models
{
    public class ProductKindsGroup
    {
        public Guid Id  { get; set; }

        public string Name { get; set; }

        public ICollection<ProductModel> Products { get; set; }

        public bool IsExpanded { get; set; }
    }
}
