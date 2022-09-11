using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Mime;
using System.Text;

namespace Shopping.SpecFlow.Extensions
{
    public static class ObjectExtensions
    {
        public static HttpContent ToHttpContent(this object obj)
        {
            var contractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
            var serializeSettings = new JsonSerializerSettings { ContractResolver = contractResolver };
            var serializedContent = JsonConvert.SerializeObject(obj, serializeSettings);
            return new StringContent(serializedContent, Encoding.UTF8, MediaTypeNames.Application.Json);
        }
    }
}