using Microsoft.Extensions.Localization;
using Shopping.Client.Shared.ResourceFiles;

namespace Shopping.Client.Pages
{
    // implement feature to suggest three similar products
    public partial class MergeProductComponent : ComponentBase
    {
        [Parameter]
        public ProductModel ProductToMerge { get; set; }

        [Parameter]
        public IReadOnlyCollection<ProductModel> AllProducts { get; set; } = Array.Empty<ProductModel>();

        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected IStringLocalizer<Resource> Localizer { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        private IEnumerable<ProductModel> _products => AllProducts.Where(p => p.Id != ProductToMerge.Id).ToArray();

        private ProductModel _secondProduct { get; set; }

        private async Task Merge()
        {
            var request = new MergeProduct
            {
                SavedProductId = ProductToMerge.Id,
                RemovedProductId = _secondProduct.Id,
            };

            var response = await HttpClient.PostAsJsonAsync("/api/products/merged", request);
            if (response.IsSuccessStatusCode)
            {
                DialogService.Close(true);
            }
        }

        private void Close()
        {
            DialogService.Close();
        }
    }
}