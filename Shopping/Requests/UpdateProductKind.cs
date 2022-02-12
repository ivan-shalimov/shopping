using MediatR;

namespace Shopping.Requests
{
    public sealed class UpdateProductKind : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = String.Empty;
    }
}