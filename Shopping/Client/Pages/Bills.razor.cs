using Microsoft.Extensions.Localization;
using Shopping.Client.Shared.ResourceFiles;
using Shopping.Shared.Requests.Bills;

namespace Shopping.Client.Pages
{
    public partial class Bills : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected IStringLocalizer<Resource> Localizer { get; set; }

        private bool _pending = true;
        private BillModel[] _list = Array.Empty<BillModel>();

        private int _newBillYear = DateTime.UtcNow.Month == 1 ? DateTime.UtcNow.Year - 1 : DateTime.UtcNow.Year;
        private int _newBillMonth = DateTime.UtcNow.Month == 1 ? 12 : DateTime.UtcNow.Month - 1;

        protected override async Task OnParametersSetAsync()
        {
            await Reload().ConfigureAwait(false);
            await base.OnParametersSetAsync();
        }

        private async Task Reload()
        {
            _pending = true;
            _list = await HttpClient.GetFromJsonAsync<BillModel[]>($"/api/bills");
            _pending = false;
        }

        private async Task CreateBill(DialogService ds)
        {
            if (2020 > _newBillYear || _newBillYear > DateTime.UtcNow.Year || _newBillMonth < 1 || _newBillMonth > 12)
            {
                await DialogService.Alert(Localizer["IncorrectYearOrMonth"], Localizer["ErrorTitle"], new AlertOptions() { OkButtonText = "Yes" });
                return;
            }

            ds.Close(true);
            await HttpClient.PostAsJsonAsync("/api/bills", new CreateBill
            {
                Month = _newBillMonth,
                Year = _newBillYear,
                Description = string.Format(Localizer["BillTitleFormat"], _newBillYear, _newBillMonth),
            });
            await Reload().ConfigureAwait(false);
        }

        private async Task Delete(BillModel billModel)
        {
            await HttpClient.DeleteAsync($"/api/bills/{billModel.Id}");
            await Reload().ConfigureAwait(false);
        }
    }
}