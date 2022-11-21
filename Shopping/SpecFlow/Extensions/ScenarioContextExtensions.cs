using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Shopping.SpecFlow.Extensions
{
    public static class ScenarioContextExtensions
    {
        public static T GetValueOrDefault<T>(this ScenarioContext scenarioContext, string key)
        {
            return scenarioContext.TryGetValue<T>(key, out var value) ? value : default;
        }

        public static List<T> GetCurrentOrEmptyCollection<T>(this ScenarioContext scenarioContext, string key)
        {
            return scenarioContext.TryGetValue<List<T>>(key, out var value) ? value : new List<T>();
        }

        public static T GetDeserializedValueOrDefault<T>(this ScenarioContext scenarioContext, string key) where T : class, new()
        {
            var value = scenarioContext.GetValueOrDefault<string>(key);
            return JsonConvert.DeserializeObject<T>(value);
        }

        public static T[] GetDeserializedCollectionOrEmpty<T>(this ScenarioContext scenarioContext, string key)
        {
            var value = scenarioContext.GetValueOrDefault<string>(key);
            try
            {
                return JsonConvert.DeserializeObject<T[]>(value);
            }
            catch
            {
                return Array.Empty<T>();
            }
        }

        public static Guid GetValueOrRandom(this ScenarioContext scenarioContext, string key)
        {
            return scenarioContext.TryGetValue<Guid>(key, out var value) ? value : Guid.NewGuid();
        }

        public static string FillInterpolatedString(this ScenarioContext scenarioContext, string path)
        {
            var queryDelimiter = path.IndexOf("?");
            if (queryDelimiter != -1)
            {
                throw new InvalidOperationException("Query should be specified as table");
            }
            var regexp = new Regex("{[^{]*}");
            var parameters = regexp.Matches(path);
            foreach (Match parameter in parameters)
            {
                var contextKey = parameter.Value.Substring(1, parameter.Length - 2);
                path = path.Replace(parameter.Value, scenarioContext[contextKey].ToString());
            }
            return path;
        }
    }
}