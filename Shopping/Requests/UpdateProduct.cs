using MediatR;

namespace Shopping.Requests
{
    public sealed class UpdateProduct : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ProductKindId { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;
    }
}