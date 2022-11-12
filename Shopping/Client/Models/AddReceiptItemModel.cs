namespace Shopping.Client.Models
{
    public class AddReceiptItemModel : EditReceiptItemModel
    {
        private Guid? _productId;

        public Guid? ProductId
        {
            get => _productId;
            set
            {
                if (_productId != value)
                {
                    IsSubmitted = false;
                }
                _productId = value;
            }
        }

        public string ProductName { get; set; } = string.Empty;

        public bool IsProductRequired => !ProductId.HasValue && IsSubmitted;

        public override bool IsValid => base.IsValid && !IsProductRequired;
    }
}