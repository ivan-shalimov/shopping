using System.Globalization;

namespace Shopping.Client.Pages
{
    public partial class Dashboard : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        public string PreviousMonthTotal { get; set; }
        public string CurrentMonthTotal { get; set; }

        public IDictionary<string, decimal> PreviousMonth { get; set; }

        public IDictionary<string, decimal> CurrentMonth { get; set; }

        public IDictionary<string, decimal> LastYearOutlay { get; set; }

        public IDictionary<string, decimal> CurrentYearOutlay { get; set; }

        public IDictionary<string, decimal> Shops { get; set; }

        public IEnumerable<ProductCostChange> ProductCostChanges { get; set; }

        protected override async Task OnInitializedAsync()
        {
            PreviousMonth = await HttpClient.GetFromJsonAsync<IDictionary<string, decimal>>("/api/statistic/expenses-by-kind/previous/month");
            PreviousMonthTotal = FormatAsUAN(PreviousMonth.Values.Sum(x => x));
            CurrentMonth = await HttpClient.GetFromJsonAsync<IDictionary<string, decimal>>("/api/statistic/expenses-by-kind/current/month");
            CurrentMonthTotal = FormatAsUAN(CurrentMonth.Values.Sum(x => x));

            var currnetMonth = DateTime.UtcNow.Month;
            var lastYearExpenses = await HttpClient.GetFromJsonAsync<IDictionary<int, decimal>>("/api/statistic/expenses-by-month/previous/year");
            LastYearOutlay = lastYearExpenses
                .Where(x => x.Key <= currnetMonth)
                .Take(4)
                .ToDictionary(x => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Key), x => x.Value);
            var currentYearExpenses = await HttpClient.GetFromJsonAsync<IDictionary<int, decimal>>("/api/statistic/expenses-by-month/current/year");
            CurrentYearOutlay = currentYearExpenses
                .Where(x => x.Key <= currnetMonth)
                .Take(4)
                .ToDictionary(x => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Key), x => x.Value);

            Shops = await HttpClient.GetFromJsonAsync<IDictionary<string, decimal>>("/api/statistic/expenses-by-shop/current/month");

            ProductCostChanges = await HttpClient.GetFromJsonAsync<ProductCostChange[]>("/api/statistic/product-cost-change?page=1&pageSize=10&orderBy=percent");
            await base.OnInitializedAsync();
        }

        private string FormatAsUAN(object value) => $"{value:f2} UAN";

        private string FormatAsMonth(object value) => value switch
        {
            string str => str.Substring(0, 3),
            _ => string.Empty,
        };
    }
}