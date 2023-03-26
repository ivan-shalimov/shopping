using Microsoft.Extensions.Localization;
using Shopping.Client.Shared.ResourceFiles;

namespace Shopping.Client.Pages
{
    public partial class ReceiptItems : ComponentBase
    {
        [Parameter]
        public string ReceiptId { get; set; }

        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected IStringLocalizer<Resource> Localizer { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        private ExtendedReceiptItemModel[]? _list;

        protected override async Task OnInitializedAsync()
        {
            _list = null;
            var response = await HttpClient.GetFromJsonAsync<ReceiptItemModel[]>($"/api/receipts/{ReceiptId}/items");
            _list = response.Select(item => new ExtendedReceiptItemModel(item)).ToArray();
        }

        public async Task OpenProductSelector()
        {
            var result = await DialogService.OpenAsync<ProductSelector>(Localizer["Please select the product to add them to receipt"],
                   new Dictionary<string, object>() {
                   { nameof(ProductSelector.ReceiptProductIs), _list.Select(item => item.ReceiptItem.ProductId).ToArray() } },
                   new DialogOptions() { Width = "746px", Height = "742px", Resizable = true, Draggable = true }) as List<ProductModel>;
            if (result != null && result.Any())
            {
                var url = $"/api/prices/latest?receiptId={ReceiptId}&" + string.Join('&', result.Select(p => $"productIds={p.Id}"));
                var lastPrices = await HttpClient.GetFromJsonAsync<IDictionary<Guid, decimal>>(url);
                var temp = _list.ToList();
                temp.AddRange(result.Select(p => new ExtendedReceiptItemModel(p, lastPrices.TryGetValue(p.Id, out var price) ? price : 0m)));
                _list = temp.ToArray();
            }
        }

        private async Task Add(ExtendedReceiptItemModel newItem = null)
        {
            newItem.EditReceiptItem.IsSubmitted = true;
            if (!newItem.EditReceiptItem.IsValid)
            {
                return;
            }

            var addReceiptItemRequest = new AddReceiptItem
            {
                Id = newItem.ReceiptItem.Id,
                ReceiptId = Guid.Parse(ReceiptId), // todo investigate how to configure converting
                ProductId = newItem.ReceiptItem.ProductId,
                Price = newItem.EditReceiptItem.Price,
                Amount = newItem.EditReceiptItem.Amount,
            };

            await HttpClient.PostAsJsonAsync($"/api/receipts/{ReceiptId}/items", addReceiptItemRequest);
            newItem.ApplyChanges();
            return;
        }

        private async Task Save(ExtendedReceiptItemModel itemModel)
        {
            itemModel.EditReceiptItem.IsSubmitted = true;
            if (!itemModel.EditReceiptItem.IsValid)
            {
                return;
            }

            var request = new UpdateReceiptItem
            {
                Id = itemModel.EditReceiptItem.Id,
                Amount = itemModel.EditReceiptItem.Amount,
                Price = itemModel.EditReceiptItem.Price,
            };
            await HttpClient.PutAsJsonAsync($"/api/receipts/{ReceiptId}/items/{itemModel.EditReceiptItem.Id}", request);
            itemModel.ApplyChanges();
        }

        private async Task Delete(ExtendedReceiptItemModel itemModel)
        {
            if (!itemModel.IsNew)
            {
                await HttpClient.DeleteAsync($"/api/receipts/{ReceiptId}/items/{itemModel.ReceiptItem.Id}");
            }

            _list = _list.Where(item => item.ReceiptItem.Id != itemModel.ReceiptItem.Id).ToArray();
        }
    }
}