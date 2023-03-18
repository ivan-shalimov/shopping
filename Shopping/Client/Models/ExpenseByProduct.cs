namespace Shopping.Client.Models
{
    public class ExpenseByProduct
    {
        public string ProductName { get; set; }

        public decimal Expenses { get; set; }

        public ProductExpensesDetail[] ProductExpensesDetails { get; set; }
    }
}