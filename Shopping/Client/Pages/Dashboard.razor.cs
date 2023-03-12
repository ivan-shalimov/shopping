using Microsoft.AspNetCore.Components;
using Shopping.Shared.Models.Results;
using System.Globalization;
using System.Net.Http.Json;

namespace Shopping.Client.Pages
{
    public partial class Dashboard : ComponentBase
    {
        [Inject]
        protected HttpClient HttpClient { get; set; }

        public double PreviousMonthTotal { get; set; }
        public double CurrentMonthTotal { get; set; }

        public IDictionary<string, double> PreviousMonth { get; set; }

        public IDictionary<string, double> CurrentMonth { get; set; }

        public IDictionary<string, double> LastYearOutlay { get; set; }

        public IDictionary<string, double> CurrentYearOutlay { get; set; }

        public IDictionary<string, double> Shops { get; set; }

        public IEnumerable<ProductCostChange> ProductCostChanges { get; set; }

       /* public Dashboard()
        {
            PreviousMonthTotal = "2000 UAN";
            CurrentMonthTotal = "1500 UAN";

            PreviousMonth = new Dictionary<string, double>
            {
                { "Medical", 425 },
                { "Breed", 1250 },
                { "Meat", 425 },
                { "Fruits and vegetables", 1250 },
                { "Milk", 1005 },
                { "other", 585 },
            };

            CurrentMonth = new Dictionary<string, double>
            {
                { "Medical", 1425 },
                { "Breed", 250 },
                { "Meat", 825 },
                { "Fruits and vegetables", 250 },
                { "Milk", 1045 },
                { "other", 685 },
            };

            LastYearOutlay = new Dictionary<string, double>
            {
                { "Dec", 12158 },
                { "Jab", 10550 },
                { "Feb", 13085 },
                { "Mar", 11005 },
            };

            CurrentYearOutlay = new Dictionary<string, double>
            {
                { "Dec", 11158 },
                { "Jab", 12550 },
                { "Feb", 9085 },
                { "Mar", 425 }
            };

            Shops = new Dictionary<string, double>
            {
                { "ATB", 30 },
                { "MarketOPt", 45 },
                { "Apteka", 3 }, // min
                { "Eva", 22 }
            };

            ProductCostChanges = new[]
            {
                new ProductCostChange { Name = "Prod #1", Kind = "Kind #1", Shop = "Shop #1", PreviousCost = 125.58, LastCost=150.50 },
                new ProductCostChange { Name = "Prod #2", Kind = "Kind #2", Shop = "Shop #1", PreviousCost = 25.58, LastCost=45.25 },
                new ProductCostChange { Name = "Prod #3", Kind = "Kind #3", Shop = "Shop #2", PreviousCost = 105.58, LastCost=150.50 },
                new ProductCostChange { Name = "Prod #4", Kind = "Kind #3", Shop = "Shop #1", PreviousCost = 155.58, LastCost=150.50 },
                new ProductCostChange { Name = "Prod #5", Kind = "Kind #3", Shop = "Shop #2", PreviousCost = 155.58, LastCost=150.50 },
                new ProductCostChange { Name = "Prod #6", Kind = "Kind #5", Shop = "Shop #3", PreviousCost = 165.58, LastCost=150.50 },
                new ProductCostChange { Name = "Prod #7", Kind = "Kind #5", Shop = "Shop #1", PreviousCost = 25.58, LastCost=150.50 },
                new ProductCostChange { Name = "Prod #7", Kind = "Kind #5", Shop = "Shop #2", PreviousCost = 325.58, LastCost=150.50 },
                new ProductCostChange { Name = "Prod #8", Kind = "Kind #1", Shop = "Shop #2", PreviousCost = 525.58, LastCost=150.50 },
                new ProductCostChange { Name = "Prod #9", Kind = "Kind #1", Shop = "Shop #1", PreviousCost = 225.58, LastCost=150.50 },
            };
        }
       */

        protected override async Task OnInitializedAsync()
        {
            PreviousMonth = await HttpClient.GetFromJsonAsync<IDictionary<string, double>>("/api/statistic/expenses-by-kind/previous/month");
            PreviousMonthTotal = PreviousMonth.Values.Sum(x => x);
            CurrentMonth = await HttpClient.GetFromJsonAsync<IDictionary<string, double>>("/api/statistic/expenses-by-kind/current/month");
            CurrentMonthTotal = CurrentMonth.Values.Sum(x => x);

            var currnetMonth = DateTime.UtcNow.Month;
            // ;
            var lastYearExpenses = await HttpClient.GetFromJsonAsync<IDictionary<int, double>>("/api/statistic/expenses-by-month/previous/year");
            LastYearOutlay = lastYearExpenses
                .Where(x => x.Key <= currnetMonth)
                .Take(4)
                .ToDictionary(x => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Key), x => x.Value);
            var currentYearExpenses = await HttpClient.GetFromJsonAsync<IDictionary<int, double>>("/api/statistic/expenses-by-month/current/year");
            CurrentYearOutlay = currentYearExpenses
                .Where(x => x.Key <= currnetMonth)
                .Take(4)
                .ToDictionary(x => CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x.Key), x => x.Value);

            Shops = await HttpClient.GetFromJsonAsync<IDictionary<string, double>>("/api/statistic/expenses-by-shop/current/month");

            ProductCostChanges = await HttpClient.GetFromJsonAsync<ProductCostChange[]>("/api/statistic/product-cost-change?page=1&pageSize=10&orderBy=percent");
            await base.OnInitializedAsync();
        }
    }
}