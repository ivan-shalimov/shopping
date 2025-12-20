using Shopping.Mediator;
using Shopping.Shared.Models.Common;
using System.Text.Json.Serialization;

namespace Shopping.Shared.Requests
{
    public sealed class UpdateProductKind : IRequest<Either<Fail, Success>>
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = String.Empty;
    }
}