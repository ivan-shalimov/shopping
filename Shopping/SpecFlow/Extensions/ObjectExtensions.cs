using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Mime;
using System.Text;

namespace Shopping.SpecFlow.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly JsonSerializerSettings SerializeSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() }
        };
        public static HttpContent ToHttpContent(this object obj)
        {
            var serializedContent = JsonConvert.SerializeObject(obj, SerializeSettings);
            return new StringContent(serializedContent, Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}