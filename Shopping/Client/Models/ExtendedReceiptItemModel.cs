using Shopping.Shared.Models.Results;

namespace Shopping.Client.Models
{
    // todo consider to replace with simple model with editable state
    public class ExtendedReceiptItemModel
    {
        public ReceiptItemModel ReceiptItem { get; }

        public EditReceiptItemModel EditReceiptItem { get; private set; }

        public bool IsNew { get; private set; }

        public bool IsEditing => EditReceiptItem != null;

        public ExtendedReceiptItemModel(ReceiptItemModel receiptItem)
        {
            ReceiptItem = receiptItem;
        }

        public ExtendedReceiptItemModel(ProductModel productModel)
        {
            ReceiptItem = new ReceiptItemModel
            {
                Id = Guid.NewGuid(),
                ProductId = productModel.Id,
                ProductName = productModel.Name,
            };
            IsNew = true;
            StartEdit();
        }

        public void StartEdit()
        {
            EditReceiptItem = new EditReceiptItemModel
            {
                Id = ReceiptItem.Id,
                Amount = ReceiptItem.Amount,
                Price = ReceiptItem.Price,
            };
        }

        public void ApplyChanges()
        {
            ReceiptItem.Price = EditReceiptItem.Price;
            ReceiptItem.Amount = EditReceiptItem.Amount;
            EditReceiptItem = null;

            IsNew = false;
        }

        public void Cancel()
        {
            EditReceiptItem = null;
        }
    }
}