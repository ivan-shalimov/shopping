using MediatR;

namespace Shopping.Shared.Requests
{
    public sealed class AddReceipt : IRequest
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Description { get; set; } = string.Empty;

        public DateTime Date { get; set; }
    }
}