using MediatR;

namespace Shopping.Shared.Requests
{
    public sealed class UpdateProductKind : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = String.Empty;
    }
}