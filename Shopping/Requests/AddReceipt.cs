using MediatR;

namespace Shopping.Requests
{
    public sealed class AddReceipt : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Description { get; set; } = string.Empty;
    }
}