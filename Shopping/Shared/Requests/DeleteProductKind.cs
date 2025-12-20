using Shopping.Mediator;
using Shopping.Shared.Models.Common;
using System.Text.Json.Serialization;

namespace Shopping.Shared.Requests
{
    public sealed class DeleteProductKind : IRequest<Either<Fail, Success>>
    {
        [JsonIgnore]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}