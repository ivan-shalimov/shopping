namespace Shopping.SpecFlow.StepDefinitions
{
    [Binding]
    public class Transforms
    {
        [StepArgumentTransformation(@"(\d+)st of ([^']*) month")]
        public DateTime InDayOfMonthTransform(int dayOfMonth, string monthDescription)
        {
            var now = DateTime.Now;
            var date = monthDescription switch
            {
                "this" => new DateTime(now.Year, now.Month, dayOfMonth, 12, 0, 0),
                "the previous" => (new DateTime(now.Year, now.Month, dayOfMonth, 12, 0, 0)).AddMonths(-1),
                _ => throw new NotSupportedException(),
            };
            return date;
        }
    }
}