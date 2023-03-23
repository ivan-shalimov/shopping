using Microsoft.Extensions.Localization;
using Shopping.Client.Shared.ResourceFiles;

namespace Shopping.Client.Pages
{
    public partial class Expenses : ComponentBase
    {
        [Parameter]
        [SupplyParameterFromQuery]
        public string Period { get; set; }

        [Inject]
        protected HttpClient HttpClient { get; set; }

        [Inject]
        protected IStringLocalizer<Resource> Localizer { get; set; }

        private IEnumerable<ExpenseByKind> _expensesByKind;

        private IEnumerable<KeyValuePair<Periods, string>> _periods = Array.Empty<KeyValuePair<Periods, string>>();

        private Periods _selectedPeriod = Periods.CurrentMonth;

        private DateTime _startDate;
        private DateTime _endDate;

        private bool _hasFilterChanges = false;
        private bool _inProgress = false;

        protected override async Task OnInitializedAsync()
        {
            _periods = new[]
           {
                new KeyValuePair<Periods, string>(Periods.CurrentMonth, Localizer["Current Month"]),
                new KeyValuePair<Periods, string>(Periods.PreviousMonth,Localizer["Previous Month"]),
                new KeyValuePair<Periods, string>(Periods.CurrentYear,Localizer["Current Year"]),
                new KeyValuePair<Periods, string>(Periods.PreviousYear,Localizer["Previous Year"]),
                new KeyValuePair<Periods, string>(Periods.Custom,Localizer["Custom"]),
            };

            _selectedPeriod = Period switch
            {
                "previous-month" => Periods.PreviousMonth,
                _ => Periods.CurrentMonth
            };

            UpdatePeriodsDates();
            await Load();
            await base.OnInitializedAsync();
        }

        private async Task Load()
        {
            _hasFilterChanges = false;
            _inProgress = true;
            _expensesByKind = Array.Empty<ExpenseByKind>();

            var result = await HttpClient.GetFromJsonAsync<IDictionary<string, decimal>>($"/api/statistic/expenses-by-kinds?start={_startDate:O}&end={_endDate:O}");
            _inProgress = false;
            _expensesByKind = result
                .Select(it => new ExpenseByKind { KindName = it.Key, Expenses = it.Value })
                .OrderBy(x => x.KindName)
                .ToArray();
        }

        private void PeriodDatesChanged()
        {
            _selectedPeriod = Periods.Custom;
            _hasFilterChanges = true;
        }

        private void UpdatePeriodsDates()
        {
            var now = DateTime.UtcNow;
            _startDate = _selectedPeriod switch
            {
                Periods.CurrentMonth => new DateTime(now.Year, now.Month, 1),
                Periods.CurrentYear => new DateTime(now.Year, 1, 1),
                Periods.PreviousMonth => new DateTime(now.Year, now.Month, 1).AddMonths(-1),
                Periods.PreviousYear => new DateTime(now.Year - 1, 1, 1),
                Periods.Custom => _startDate,
                _ => throw new NotImplementedException(),
            };
            _endDate = _selectedPeriod switch
            {
                Periods.CurrentMonth => _startDate.AddMonths(1).AddSeconds(-1),
                Periods.CurrentYear => _startDate.AddYears(1),
                Periods.PreviousMonth => _startDate.AddMonths(1).AddSeconds(-1),
                Periods.PreviousYear => _startDate.AddYears(1),
                Periods.Custom => _endDate,
                _ => throw new NotImplementedException(),
            };
            _hasFilterChanges = true;
        }

        private async Task ExpenseByKindRowExpand(ExpenseByKind expenseByKind)
        {
            if (expenseByKind.ExpensesByProduct == null)
            {
                var url = $"/api/statistic/expenses-by-products?kind={expenseByKind.KindName}&start={_startDate:O}&end={_endDate:O}";
                var result = await HttpClient.GetFromJsonAsync<IDictionary<string, decimal>>(url);
                expenseByKind.ExpensesByProduct = result
                .Select(it => new ExpenseByProduct { ProductName = it.Key, Expenses = it.Value })
                .OrderBy(x => x.ProductName)
                .ToArray();
            }
        }

        private async Task ExpenseByProductRowExpand(ExpenseByProduct expenseByProduct)
        {
            if (expenseByProduct.ProductExpensesDetails == null)
            {
                var url = $"/api/statistic/product-expenses-details?productName={expenseByProduct.ProductName}&start={_startDate:O}&end={_endDate:O}";
                var result = await HttpClient.GetFromJsonAsync<ProductExpensesDetail[]>(url);
                expenseByProduct.ProductExpensesDetails = result.OrderBy(it => it.SpentOn).ToArray();
            }
        }

        private enum Periods
        {
            CurrentMonth = 1,
            PreviousMonth,
            CurrentYear,
            PreviousYear,
            Custom,
        }
    }
}