using Microsoft.AspNetCore.Components;
using Radzen;
using Shopping.Client.Models;
using Shopping.Shared.Models.Results;
using Shopping.Shared.Requests;
using System.Net.Http.Json;

namespace Shopping.Client.Pages
{
    public partial class ReceiptItems : ComponentBase
    {
        [Parameter]
        public string ReceiptId { get; set; }

        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        private ExtendedReceiptItemModel[]? _list;

        private IEnumerable<ProductModel> _products = Array.Empty<ProductModel>();
        private AddReceiptItemModel _newItem = new AddReceiptItemModel();

        protected override async Task OnInitializedAsync()
        {
            _products = await HttpClient.GetFromJsonAsync<ProductModel[]>("/api/products");
            await Reload();
        }

        public async Task OpenProductSelector()
        {
            var result = await DialogService.OpenAsync<ProductSelector>($"Please select the product to add them to receipt",
                   new Dictionary<string, object>() {
                   { nameof(ProductSelector.ReceiptProductIs), _list.Select(item => item.ReceiptItem.ProductId).ToArray() } },
                   new DialogOptions() { Width = "700px", Height = "742px", Resizable = true, Draggable = true }) as List<ProductModel>;
            if (result != null)
            {
                var temp = _list.ToList();
                temp.AddRange(result.Select(p => new ExtendedReceiptItemModel(p)));
                _list = temp.ToArray();
            }
        }

        private async Task Reload()
        {
            _list = null;
            var response = await HttpClient.GetFromJsonAsync<ReceiptItemModel[]>($"/api/receipts/{ReceiptId}/items");
            _list = response.Select(item => new ExtendedReceiptItemModel(item)).ToArray();
            if (_list == null)
            {
                return;
            }
        }

        private void ProductNameChanged(object value)
        {
            var product = _products.FirstOrDefault(p => p.Name == _newItem.ProductName);
            _newItem.ProductId = product?.Id;
        }

        private async Task Add(ExtendedReceiptItemModel newItem = null)
        {
            if (newItem != null)
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

            _newItem.IsSubmitted = true;
            if (!_newItem.IsValid)
            {
                return;
            }

            var request = new AddReceiptItem
            {
                Id = Guid.NewGuid(),
                ReceiptId = Guid.Parse(ReceiptId), // todo investigate how to configure converting
                ProductId = _newItem.ProductId.Value,
                Price = _newItem.Price,
                Amount = _newItem.Amount,
            };

            await HttpClient.PostAsJsonAsync($"/api/receipts/{ReceiptId}/items", request);

            _newItem = new AddReceiptItemModel();

            await Reload().ConfigureAwait(false);
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