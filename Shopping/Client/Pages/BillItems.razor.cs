using Microsoft.Extensions.Localization;
using Shopping.Client.Shared.ResourceFiles;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Client.Pages
{
    public partial class BillItems : ComponentBase
    {
        [Parameter]
        public string BillId { get; set; }

        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected IStringLocalizer<Resource> Localizer { get; set; }

        private int? value;

        private bool _pending = false;
        private Dictionary<string, BillItemModel[]> _items = new Dictionary<string, BillItemModel[]>();

        protected override async Task OnParametersSetAsync()
        {
            await Reload().ConfigureAwait(false);
            await base.OnParametersSetAsync();
        }

        private async Task Reload()
        {
            _pending = true;
            var result = await HttpClient.GetFromJsonAsync<BillItemModel[]>($"/api/bills/{BillId}/items");
            _items = result
                .OrderBy(e=>e.Group)
                .GroupBy(x => x.Group)
                .ToDictionary(x => x.Key, x => x.OrderBy(e=>e.Description).ToArray());
            _pending = false;
        }

        public async Task UpdateQuantity(BillItemModel billItem, int currentValue)
        {
            var quantity = currentValue - billItem.PreviousValue;
            if (quantity < 0)
            {
                await DialogService.Alert(Localizer["IncorrectQuantity"], Localizer["ErrorTitle"], new AlertOptions() { OkButtonText = "Yes" });
                return;
            }

            var request = new UpdateBillItemQuantity { Quantity = quantity, };
            await HttpClient.PutAsJsonAsync($"/api/bills/{billItem.BillId}/items/{billItem.Id}/quantity", request);
            billItem.CurrentValue = currentValue;

            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Info, Summary = Localizer["Saved successfully"], Detail = $"{billItem.Group}: {billItem.Description}", Duration = 4000 });
        }

        public async Task UpdateRate(BillItemModel billItem, decimal value)
        {
            if (value < 0)
            {
                await DialogService.Alert(Localizer["IncorrectRate"], Localizer["ErrorTitle"], new AlertOptions() { OkButtonText = "Yes" });
                return;
            }

            var request = new UpdateBillItemRate { Rate = value, };
            await HttpClient.PutAsJsonAsync($"/api/bills/{billItem.BillId}/items/{billItem.Id}/rate", request);
            billItem.Rate = value;

            NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Info, Summary = Localizer["Saved successfully"], Detail = $"{billItem.Group}: {billItem.Description}", Duration = 4000 });
        }
    }
}