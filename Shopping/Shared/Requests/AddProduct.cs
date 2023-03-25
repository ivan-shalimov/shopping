using MediatR;

namespace Shopping.Shared.Requests
{
    public sealed class AddProduct : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProductKindId { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
        public string Type { get; set; }
    }
}