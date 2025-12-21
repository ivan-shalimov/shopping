using Reqnroll.Assist;

namespace Shopping.SpecFlow.ValueRetrievers
{
    public class MonthNumberValueRetriever : IValueRetriever
    {
        public virtual int GetValue(string value)
        {
            if (value.StartsWith("current"))
            {
                return DateTime.UtcNow.Month;
            }
            else if (value.StartsWith("previous"))
            {
                return DateTime.UtcNow.AddMonths(-1).Month;
            }

            throw new NotSupportedException();
        }

        public object Retrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return GetValue(keyValuePair.Value);
        }

        public bool CanRetrieve(KeyValuePair<string, string> keyValuePair, Type targetType, Type propertyType)
        {
            return keyValuePair.Value?.EndsWith("month number") == true;
        }
    }
}