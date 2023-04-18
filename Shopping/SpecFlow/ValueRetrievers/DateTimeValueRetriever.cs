using System.Text.RegularExpressions;
using TechTalk.SpecFlow.Assist;

namespace Shopping.SpecFlow.ValueRetrievers
{
    public class DateTimeValueRetriever : IValueRetriever
    {
        private static readonly Regex RegexDateDescription = new Regex(@"(\d+)st of ([^']*) month");

        public virtual DateTime GetValue(string value)
        {
            var groups = RegexDateDescription.Match(value).Groups;
            var now = DateTime.Now;
            var date = groups[2].Value switch
            {
                "this" => new DateTime(now.Year, now.Month, int.Parse(groups[1].Value), 12, 0, 0),
                "the previous" => (new DateTime(now.Year, now.Month, int.Parse(groups[1].Value), 12, 0, 0)).AddMonths(-1),
                _ => throw new NotSupportedException(),
            };
            return date;
        }

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return GetValue(keyValuePair.Value);
        }

        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return propertyType == typeof(DateTime) && RegexDateDescription.Match(keyValuePair.Value).Success;
        }
    }
}