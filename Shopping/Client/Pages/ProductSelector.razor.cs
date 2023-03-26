using Microsoft.Extensions.Localization;
using Shopping.Client.Shared.ResourceFiles;

namespace Shopping.Client.Pages
{
    public partial class ProductSelector : ComponentBase
    {
        [Parameter]
        public IReadOnlyCollection<Guid> ReceiptProductIs { get; set; } = Array.Empty<Guid>();

        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected IStringLocalizer<Resource> Localizer { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        private List<ProductModel> _allProducts = new List<ProductModel>();

        private Dictionary<Guid, string> _groups = new Dictionary<Guid, string>();
        private Guid _selectedGroup = Guid.Empty;

        private Dictionary<Guid, string> _products = new Dictionary<Guid, string>();
        private List<Guid> _selectedProductIds = new List<Guid>();

        protected override async Task OnInitializedAsync()
        {
            _allProducts = await HttpClient.GetFromJsonAsync<List<ProductModel>>($"/api/products");

            if (!_allProducts.Any())
            {
                return;
            }

            _groups = _allProducts.GroupBy(x => x.ProductKindId)
                .Select(it => new { it.Key, it.First().ProductKindName })
                .OrderBy(it => it.ProductKindName)
                .ToDictionary(it => it.Key, it => it.ProductKindName.Length > 16 ? it.ProductKindName.Substring(0, 16) : it.ProductKindName);

            SelectGroup(_groups.First().Key);

            await base.OnInitializedAsync();
        }

        public void SelectGroup(Guid groupId)
        {
            _selectedGroup = groupId;
            _products = _allProducts
                .Where(it => it.ProductKindId == groupId)
                .Select(it => new { it.Id, Name = $"{it.Type} {it.Name}" })
                .OrderBy(it => it.Name)
                .ToDictionary(it => it.Id, it => it.Name.Length > 32 ? it.Name.Substring(0, 32) : it.Name);
        }

        public void ChangeProductSelection(Guid productId)
        {
            if (_selectedProductIds.Contains(productId))
            {
                _selectedProductIds.Remove(productId);
            }
            else
            {
                _selectedProductIds.Add(productId);
            }
        }

        private void AddSelectedAndClose()
        {
            var selectedProducts = _allProducts.Where(it => _selectedProductIds.Contains(it.Id)).ToList();
            DialogService.Close(selectedProducts);
        }

        private void Close()
        {
            DialogService.Close();
        }
    }
}