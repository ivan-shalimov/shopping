namespace Shopping.Client.Models
{
    public class ExpenseByKind
    {
        public string KindName { get; set; }

        public decimal Expenses { get; set; }

        public ExpenseByProduct[] ExpensesByProduct { get; set; }
    }
}