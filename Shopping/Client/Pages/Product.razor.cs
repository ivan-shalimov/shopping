using Microsoft.Extensions.Localization;
using Shopping.Client.Shared.ResourceFiles;

namespace Shopping.Client.Pages
{
    public partial class Product : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected IStringLocalizer<Resource> Localizer { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        private bool _pending = true;
        private ProductModel[]? _list;

        private ProductKindModel[]? _productKinds;

        private Guid _allProductKindsId = Guid.Empty;
        private Guid _filterProductKindId = Guid.Empty;
        private ProductKindModel[]? _productKindsForFilter;

        private ProductModel _newItem = new ProductModel();
        private ProductModel _editedItem = null;

        protected override async Task OnInitializedAsync()
        {
            _productKinds = await HttpClient.GetFromJsonAsync<ProductKindModel[]>("/api/products/kinds");

            var filterCollection = new List<ProductKindModel>();
            filterCollection.Add(new ProductKindModel { Id = _allProductKindsId, Name = "All" });
            filterCollection.AddRange(_productKinds);
            _productKindsForFilter = filterCollection.ToArray();
            await Reload();
        }

        private async Task Reload()
        {
            _pending = true;
            var query = _filterProductKindId == Guid.Empty ? string.Empty : $"?productKindId={_filterProductKindId}";
            _list = await HttpClient.GetFromJsonAsync<ProductModel[]>($"/api/products{query}");
            _pending = false;
            if (_list == null)
            {
                return;
            }
        }

        private async Task Add()
        {
            var request = new AddProduct
            {
                Id = Guid.NewGuid(),
                Name = _newItem.Name,
                ProductKindId = _newItem.ProductKindId,
            };

            await HttpClient.PostAsJsonAsync("/api/products", request);

            _newItem = new ProductModel();

            await Reload().ConfigureAwait(false);
        }

        private void StartEdit(ProductModel product)
        {
            _editedItem = product;
        }

        private void Cancel()
        {
            _editedItem = null;
        }

        private async Task Save()
        {
            var request = new UpdateProduct { Name = _editedItem.Name, ProductKindId = _editedItem.ProductKindId, };
            await HttpClient.PutAsJsonAsync($"/api/products/{_editedItem.Id}", request);
            _editedItem = null;
            await Reload().ConfigureAwait(false);
        }

        private async Task Delete(ProductModel product)
        {
            await HttpClient.DeleteAsync($"/api/products/{product.Id}");
            await Reload().ConfigureAwait(false);
        }

        public async Task StartMergeProduct(ProductModel product)
        {
            var result = await DialogService.OpenAsync<MergeProductComponent>(Localizer["Merge products"],
                   new Dictionary<string, object>() {
                   { nameof(MergeProductComponent.ProductToMerge), product } ,
                   { nameof(MergeProductComponent.AllProducts), _list } ,
                   },
                   new DialogOptions() { Width = "700px", Height = "742px", Resizable = true, Draggable = true });
            if (result is bool success && success)
            {
                await Reload().ConfigureAwait(false);
            }
        }
    }
}