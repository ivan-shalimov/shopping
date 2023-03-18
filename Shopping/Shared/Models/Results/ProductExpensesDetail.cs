namespace Shopping.Shared.Models.Results
{
    public class ProductExpensesDetail
    {
        public string ShopName { get; set; }

        public DateTime SpentOn { get; set; }

        public decimal Price { get; set; }

        public decimal Amount { get; set; }

        public decimal Expenses => Price * Amount;
    }
}