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

        private RadzenTree radzenTree;

        private List<ProductKindsGroup> _kindWithProducts = new List<ProductKindsGroup>();

        private List<ProductModel> _checkedProducts = new List<ProductModel>();

        private IEnumerable<object> _checkedValues;

        protected override async Task OnInitializedAsync()
        {
            var allProducts = await HttpClient.GetFromJsonAsync<List<ProductModel>>($"/api/products");
            _kindWithProducts = allProducts
                .Where(product => !ReceiptProductIs.Contains(product.Id))
                .GroupBy(x => x.ProductKindId)
                .Select(gr => new ProductKindsGroup { Id = gr.Key, Name = gr.First().ProductKindName, Products = gr.ToList() })
                .OrderBy(k => k.Name).ToList();

            await base.OnInitializedAsync();
        }

        private void OnChange(TreeEventArgs args)
        {
            if (args.Value is ProductModel productModel)
            {
                if (_checkedValues.Contains(productModel))
                {
                    _checkedProducts.Remove(productModel);
                }
                else
                {
                    _checkedProducts.Add(productModel);
                }
            }

            if (args.Value is ProductKindsGroup kindsGroup)
            {
                _kindWithProducts.ForEach(kind => kind.IsExpanded = kind.Id == kindsGroup.Id ? !kind.IsExpanded : false);
            }

            _checkedValues = _checkedProducts;
            radzenTree.ClearSelection();
        }

        private void AddSelectedAndClose()
        {
            DialogService.Close(_checkedValues.Where(v => v is ProductModel).Cast<ProductModel>().ToList());
        }

        private void Close()
        {
            DialogService.Close();
        }
    }
}