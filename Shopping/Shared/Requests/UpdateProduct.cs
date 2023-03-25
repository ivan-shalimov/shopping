using MediatR;
using System.Text.Json.Serialization;

namespace Shopping.Shared.Requests
{
    public sealed class UpdateProduct : IRequest
    {

        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProductKindId { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string? Type { get; set; }
    }
}