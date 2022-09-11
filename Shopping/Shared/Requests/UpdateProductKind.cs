using MediatR;
using System.Text.Json.Serialization;

namespace Shopping.Shared.Requests
{
    public sealed class UpdateProductKind : IRequest
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = String.Empty;
    }
}