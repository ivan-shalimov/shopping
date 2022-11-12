namespace Shopping.SpecFlow.Extensions
{
    public static class TableExtensions
    {
        public static string GetInterpolatedQuery(this Table table, ScenarioContext scenarioContext)
        {
            if (table.RowCount != 1)
            {
                throw new InvalidDataException($"{nameof(table)} should contain only one row");
            }
            var row = table.Rows[0];
            return string.Join("&", row.Select(kv =>
            {
                if (kv.Value.StartsWith("{") && kv.Value.EndsWith("}"))
                {
                    var key = kv.Value.Substring(1, kv.Value.Length - 2);
                    var value = scenarioContext.TryGetValue(key, out var v) ? v.ToString() : string.Empty;
                    return $"{kv.Key}={value}";
                }
                else
                {
                    return $"{kv.Key}={kv.Value}";
                }
            }));
        }

        public static T GetInstance<T>(this Table table, ScenarioContext scenarioContext) where T : class, new()
        {
            if (table.RowCount != 1)
            {
                throw new InvalidDataException($"{nameof(table)} should contain only one row");
            }
            var row = table.Rows[0];
            var obj = new T();
            foreach (var prop in obj.GetType().GetProperties())
            {
                var value = row.TryGetValue(prop.Name, out var v) ? v : null;
                if (value != null)
                {
                    if (value.StartsWith("{") && value.EndsWith("}"))
                    {
                        var key = value.Substring(1, value.Length - 2);
                        if (prop.PropertyType.Equals(typeof(string)))
                        {
                            var stringValue = scenarioContext.TryGetValue<string>(key, out var sv) ? sv : default;
                            prop.SetValue(obj, stringValue);
                        }
                        else if (prop.PropertyType.Equals(typeof(DateTime)))
                        {
                            var dateTimeValue = scenarioContext.TryGetValue<DateTime>(key, out var dt) ? dt : default;
                            prop.SetValue(obj, dateTimeValue);
                        }
                        else if (prop.PropertyType.Equals(typeof(Guid)))
                        {
                            var guidValue = scenarioContext.TryGetValue<Guid>(key, out var gv) ? gv : default;
                            prop.SetValue(obj, guidValue);
                        }
                        else if (prop.PropertyType.Equals(typeof(Guid?)))
                        {
                            var nullabelGuidValue = scenarioContext.TryGetValue<Guid?>(key, out var gv) ? gv : default;
                            prop.SetValue(obj, nullabelGuidValue);
                        }
                    }
                    else
                    {
                        if (prop.PropertyType.Equals(typeof(string)))
                        {
                            prop.SetValue(obj, value);
                        }
                        else if (prop.PropertyType.Equals(typeof(Guid)))
                        {
                            prop.SetValue(obj, Guid.Parse(value));
                        }
                        else if (prop.PropertyType.Equals(typeof(decimal)))
                        {
                            prop.SetValue(obj, decimal.Parse(value));
                        }
                    }
                }
            }
            return obj;
        }
    }
}